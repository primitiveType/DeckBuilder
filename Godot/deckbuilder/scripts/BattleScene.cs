using System;
using System.Collections.Generic;
using Api;
using Godot;

namespace Deckbuilder;

public partial class BattleScene : Node3D
{
    private const string ContentRoot = "res://../../Cards/SummerJam1/StreamingAssets";
    private const float PlayAboveHandThreshold = 92.0f;

    private readonly Dictionary<CardView, Vector2> _cardPositions = new();
    private readonly Dictionary<CardView, float> _cardRotations = new();
    private readonly Dictionary<CardView, float> _cardScales = new();
    private readonly Dictionary<CardView, CardTargetMode> _cardTargetModes = new();
    private readonly List<CardView> _cardViews = new();
    private readonly List<EnemyView> _enemyViews = new();

    private Camera3D _camera = null!;
    private CardView? _draggedCard;
    private CardView? _hoveredCard;
    private Control _handRoot = null!;
    private GameRuntime _runtime = null!;
    private Label _battleStatus = null!;
    private Node3D _enemyRoot = null!;
    private PackedScene _cardViewScene = null!;
    private PackedScene _enemyViewScene = null!;
    private TargetingArrow _targetingArrow = null!;
    private Vector2 _dragOffset;
    private bool _isDragging;

    public override void _Ready()
    {
        _cardViewScene = ResourceLoader.Load<PackedScene>("res://scenes/card_view.tscn");
        _enemyViewScene = ResourceLoader.Load<PackedScene>("res://scenes/enemy_view.tscn");

        BuildWorld();
        BuildOverlay();
        BootBattle();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMSizeChanged && _handRoot != null)
        {
            LayoutHand();
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseMotion motion)
        {
            if (_draggedCard != null)
            {
                UpdateDraggedCard(motion.Position);
                return;
            }

            UpdateHoveredCard(motion.Position);
            return;
        }

        if (inputEvent is not InputEventMouseButton { ButtonIndex: MouseButton.Left } mouseButton)
        {
            return;
        }

        if (mouseButton.Pressed)
        {
            CardView? card = FindTopCardAt(mouseButton.Position);
            if (card == null)
            {
                return;
            }

            BeginCardDrag(card, mouseButton.Position);
            GetViewport().SetInputAsHandled();
            return;
        }

        if (_draggedCard != null)
        {
            EndCardDrag(mouseButton.Position);
            GetViewport().SetInputAsHandled();
        }
    }

    private void BuildWorld()
    {
        _camera = new Camera3D
        {
            Name = "Camera3D",
            Position = new Vector3(0, 5.2f, 8.8f),
            RotationDegrees = new Vector3(-30.0f, 0, 0),
            Current = true,
            Fov = 45.0f
        };
        AddChild(_camera);

        var light = new DirectionalLight3D
        {
            Name = "KeyLight",
            RotationDegrees = new Vector3(-55.0f, -28.0f, 0),
            LightEnergy = 2.2f
        };
        AddChild(light);

        var floor = new MeshInstance3D
        {
            Name = "BattleFloor",
            Mesh = new PlaneMesh { Size = new Vector2(11.5f, 7.0f) },
            Position = new Vector3(0, -0.02f, -0.55f)
        };
        var floorMaterial = new StandardMaterial3D
        {
            AlbedoColor = new Color(0.18f, 0.19f, 0.18f),
            Roughness = 0.82f
        };
        floor.MaterialOverride = floorMaterial;
        AddChild(floor);

        _enemyRoot = new Node3D
        {
            Name = "EnemyRoot",
            Position = new Vector3(0, 0, -1.8f)
        };
        AddChild(_enemyRoot);
    }

    private void BuildOverlay()
    {
        var canvasLayer = new CanvasLayer { Name = "BattleOverlay" };
        AddChild(canvasLayer);

        var overlay = new Control { Name = "Overlay" };
        overlay.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        overlay.OffsetLeft = 0;
        overlay.OffsetTop = 0;
        overlay.OffsetRight = 0;
        overlay.OffsetBottom = 0;
        canvasLayer.AddChild(overlay);

        var topBar = new HBoxContainer
        {
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 0,
            OffsetLeft = 18,
            OffsetTop = 16,
            OffsetRight = -18,
            OffsetBottom = 58
        };
        topBar.AddThemeConstantOverride("separation", 10);
        overlay.AddChild(topBar);

        var backButton = new Button
        {
            Text = "Map",
            CustomMinimumSize = new Vector2(88, 38)
        };
        backButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/map_scene.tscn");
        topBar.AddChild(backButton);

        var endTurnButton = new Button
        {
            Text = "End Turn",
            CustomMinimumSize = new Vector2(118, 38)
        };
        endTurnButton.Pressed += () => _runtime.EndTurn();
        topBar.AddChild(endTurnButton);

        _battleStatus = new Label
        {
            Text = "Starting battle...",
            VerticalAlignment = VerticalAlignment.Center,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        _battleStatus.AddThemeFontSizeOverride("font_size", 16);
        topBar.AddChild(_battleStatus);

        _handRoot = new Control
        {
            Name = "HandRoot",
            AnchorLeft = 0,
            AnchorTop = 1,
            AnchorRight = 1,
            AnchorBottom = 1,
            OffsetLeft = 0,
            OffsetTop = -320,
            OffsetRight = 0,
            OffsetBottom = 0,
            ClipContents = false
        };
        overlay.AddChild(_handRoot);

        _targetingArrow = new TargetingArrow { Name = "TargetingArrow" };
        overlay.AddChild(_targetingArrow);
    }

    private void BootBattle()
    {
        _runtime = new GameRuntime { Name = "GameRuntime" };
        AddChild(_runtime);
        _runtime.HandChanged += RefreshBattleViews;
        _runtime.SnapshotChanged += RefreshStatus;

        string contentRoot = ProjectSettings.GlobalizePath(ContentRoot);
        _runtime.Boot(contentRoot);
        _runtime.StartBattle();
        RefreshBattleViews();
    }

    private void RefreshBattleViews()
    {
        RefreshStatus();
        RefreshEnemies();
        RefreshHand();
    }

    private void RefreshStatus()
    {
        if (_battleStatus == null || _runtime == null)
        {
            return;
        }

        _battleStatus.Text = $"Enemies: {_runtime.GetEnemyUnits().Count}  |  Hand: {_runtime.GetHandCards().Count}";
    }

    private void RefreshEnemies()
    {
        foreach (Node child in _enemyRoot.GetChildren())
        {
            child.Free();
        }
        _enemyViews.Clear();

        IReadOnlyList<IEntity> enemies = _runtime.GetEnemyUnits();
        if (enemies.Count == 0)
        {
            return;
        }

        float spacing = Mathf.Min(2.4f, 7.0f / Mathf.Max(1, enemies.Count - 1));
        float startX = -spacing * (enemies.Count - 1) / 2.0f;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemyView = _enemyViewScene.Instantiate<EnemyView>();
            enemyView.Position = new Vector3(startX + spacing * i, 0, 0);
            enemyView.Bind(enemies[i]);
            _enemyRoot.AddChild(enemyView);
            _enemyViews.Add(enemyView);
        }
    }

    private void RefreshHand()
    {
        foreach (Node child in _handRoot.GetChildren())
        {
            child.Free();
        }

        _cardViews.Clear();
        _cardPositions.Clear();
        _cardRotations.Clear();
        _cardScales.Clear();
        _cardTargetModes.Clear();
        _hoveredCard = null;
        _draggedCard = null;
        _isDragging = false;
        _targetingArrow?.HideArrow();

        foreach (IEntity card in _runtime.GetHandCards())
        {
            var cardView = _cardViewScene.Instantiate<CardView>();
            cardView.Bind(card);
            cardView.PivotOffset = cardView.CustomMinimumSize / 2.0f;
            _handRoot.AddChild(cardView);
            _cardViews.Add(cardView);
            _cardTargetModes[cardView] = _runtime.GetCardTargetMode(card);
        }

        LayoutHand();
    }

    private void LayoutHand()
    {
        if (_handRoot == null || _cardViews.Count == 0)
        {
            return;
        }

        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
        float cardWidth = 190.0f;
        float cardHeight = 250.0f;
        float usableWidth = Mathf.Max(320.0f, viewportSize.X - 180.0f);
        float unscaledFanWidth = cardWidth + 132.0f * Mathf.Max(0, _cardViews.Count - 1);
        float baseScale = Mathf.Clamp(usableWidth / unscaledFanWidth, 0.68f, 1.0f);
        float spacing = _cardViews.Count <= 1
            ? 0.0f
            : 132.0f * baseScale;
        float scaledCardWidth = cardWidth * baseScale;
        float scaledCardHeight = cardHeight * baseScale;
        float startX = viewportSize.X * 0.5f - spacing * (_cardViews.Count - 1) * 0.5f - scaledCardWidth * 0.5f;
        float baseY = _handRoot.Size.Y - scaledCardHeight - 18.0f;
        float center = (_cardViews.Count - 1) * 0.5f;

        for (int i = 0; i < _cardViews.Count; i++)
        {
            CardView cardView = _cardViews[i];
            float offsetFromCenter = i - center;
            float normalized = _cardViews.Count <= 1 ? 0.0f : offsetFromCenter / center;
            float rotation = normalized * 9.0f;
            float y = baseY + Mathf.Abs(normalized) * 34.0f;
            var targetPosition = new Vector2(startX + spacing * i, y);

            _cardPositions[cardView] = targetPosition;
            _cardRotations[cardView] = rotation;
            _cardScales[cardView] = baseScale;

            cardView.ZIndex = i;
            if (cardView != _draggedCard)
            {
                AnimateCard(cardView, targetPosition, rotation, new Vector2(baseScale, baseScale), 0.18f);
            }
        }
    }

    private void FocusCard(CardView cardView)
    {
        if (!_cardPositions.TryGetValue(cardView, out Vector2 position))
        {
            return;
        }

        cardView.ZIndex = 100;
        var focusPosition = GetFocusedCardPosition(cardView);
        float focusScale = GetFocusedCardScale(cardView);
        AnimateCard(cardView, focusPosition, 0.0f, new Vector2(focusScale, focusScale), 0.13f);
    }

    private void UnfocusCard(CardView cardView)
    {
        if (!_cardPositions.TryGetValue(cardView, out Vector2 position))
        {
            return;
        }

        int handIndex = _cardViews.IndexOf(cardView);
        cardView.ZIndex = Mathf.Max(0, handIndex);
        float rotation = _cardRotations.GetValueOrDefault(cardView, 0.0f);
        float baseScale = _cardScales.GetValueOrDefault(cardView, 1.0f);
        AnimateCard(cardView, position, rotation, new Vector2(baseScale, baseScale), 0.12f);
    }

    private void UpdateHoveredCard(Vector2 mousePosition)
    {
        CardView? nextHovered = FindTopCardAt(mousePosition);
        if (nextHovered == _hoveredCard)
        {
            return;
        }

        if (_hoveredCard != null)
        {
            UnfocusCard(_hoveredCard);
        }

        _hoveredCard = nextHovered;
        if (_hoveredCard != null)
        {
            FocusCard(_hoveredCard);
        }
    }

    private CardView? FindTopCardAt(Vector2 mousePosition)
    {
        CardView? topCard = null;
        int topZ = int.MinValue;

        foreach (CardView cardView in _cardViews)
        {
            if (!cardView.GetGlobalRect().Grow(6.0f).HasPoint(mousePosition))
            {
                continue;
            }

            if (cardView.ZIndex >= topZ)
            {
                topZ = cardView.ZIndex;
                topCard = cardView;
            }
        }

        return topCard;
    }

    private void BeginCardDrag(CardView cardView, Vector2 mousePosition)
    {
        _draggedCard = cardView;
        _hoveredCard = cardView;
        _isDragging = true;
        _dragOffset = mousePosition - cardView.GlobalPosition;
        cardView.ZIndex = 1000;
        if (UsesArrowTargeting(cardView))
        {
            FocusCard(cardView);
            _targetingArrow.ShowArrow(GetFocusedCardCenter(cardView), mousePosition);
        }
        else
        {
            _targetingArrow.HideArrow();
            cardView.RotationDegrees = 0.0f;
            float baseScale = _cardScales.GetValueOrDefault(cardView, 1.0f);
            float dragScale = Mathf.Min(1.12f, baseScale * 1.14f);
            cardView.Scale = new Vector2(dragScale, dragScale);
        }
    }

    private void UpdateDraggedCard(Vector2 mousePosition)
    {
        if (_draggedCard == null)
        {
            return;
        }

        if (UsesArrowTargeting(_draggedCard))
        {
            _targetingArrow.ShowArrow(GetFocusedCardCenter(_draggedCard), mousePosition);
            return;
        }

        _draggedCard.GlobalPosition = mousePosition - _dragOffset;
        _draggedCard.RotationDegrees = 0.0f;
    }

    private void EndCardDrag(Vector2 mousePosition)
    {
        CardView? cardView = _draggedCard;
        _draggedCard = null;
        _targetingArrow.HideArrow();

        if (cardView == null)
        {
            return;
        }

        if (_isDragging && cardView.BoundCard != null && UsesArrowTargeting(cardView))
        {
            EnemyView? target = FindEnemyAt(mousePosition);
            if (target?.BoundEnemy != null && _runtime.TryPlayCard(cardView.BoundCard, target.BoundEnemy))
            {
                _isDragging = false;
                return;
            }
        }
        else if (_isDragging && cardView.BoundCard != null && WasReleasedAboveHand(cardView))
        {
            if (_runtime.TryPlayCardWithDefaultTarget(cardView.BoundCard))
            {
                _isDragging = false;
                return;
            }
        }

        _isDragging = false;
        LayoutHand();
        UpdateHoveredCard(mousePosition);
    }

    private EnemyView? FindEnemyAt(Vector2 mousePosition)
    {
        EnemyView? nearest = null;
        float nearestDistance = 72.0f;

        foreach (EnemyView enemyView in _enemyViews)
        {
            Vector2 screenPosition = _camera.UnprojectPosition(enemyView.GlobalPosition + new Vector3(0, 0.95f, 0));
            float distance = screenPosition.DistanceTo(mousePosition);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemyView;
            }
        }

        return nearest;
    }

    private bool UsesArrowTargeting(CardView cardView)
    {
        return _cardTargetModes.GetValueOrDefault(cardView, CardTargetMode.Self) == CardTargetMode.OneEnemy;
    }

    private bool WasReleasedAboveHand(CardView cardView)
    {
        if (!_cardPositions.TryGetValue(cardView, out Vector2 restingPosition))
        {
            return false;
        }

        float cardTopInHand = cardView.GlobalPosition.Y - _handRoot.GlobalPosition.Y;
        return cardTopInHand <= restingPosition.Y - PlayAboveHandThreshold;
    }

    private void AnimateCard(CardView cardView, Vector2 position, float rotationDegrees, Vector2 scale, float duration)
    {
        Tween tween = CreateTween().SetParallel(true);
        tween.TweenProperty(cardView, "position", position, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        tween.TweenProperty(cardView, "rotation_degrees", rotationDegrees, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        tween.TweenProperty(cardView, "scale", scale, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
    }

    private Vector2 GetFocusedCardCenter(CardView cardView)
    {
        Vector2 position = GetFocusedCardPosition(cardView);
        float scale = GetFocusedCardScale(cardView);
        return _handRoot.GlobalPosition + position + cardView.Size * scale * 0.5f;
    }

    private Vector2 GetFocusedCardPosition(CardView cardView)
    {
        Vector2 position = _cardPositions.GetValueOrDefault(cardView, cardView.GlobalPosition);
        float baseScale = _cardScales.GetValueOrDefault(cardView, 1.0f);
        return new Vector2(position.X, Mathf.Max(6.0f, position.Y - 92.0f * baseScale));
    }

    private float GetFocusedCardScale(CardView cardView)
    {
        float baseScale = _cardScales.GetValueOrDefault(cardView, 1.0f);
        return Mathf.Min(1.16f, baseScale * 1.18f);
    }
}
