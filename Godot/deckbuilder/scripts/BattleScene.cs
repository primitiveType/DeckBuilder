using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Godot;
using SummerJam1;
using SummerJam1.Characters;
using SummerJam1.Statuses;

namespace Deckbuilder;

public partial class BattleScene : Node3D
{
    private const string ContentRoot = "res://../../Cards/SummerJam1/StreamingAssets";
    private const float ReferenceWidth = 1280.0f;
    private const float ReferenceHeight = 720.0f;
    private const float PlayAboveHandThreshold = 92.0f;
    private const float PartyPanelWidth = 408.0f;
    private const float HandReservedHeight = 400.0f;
    private const float HandBottomMargin = 84.0f;

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
    private GridContainer _partySlots = null!;
    private HBoxContainer _topBar = null!;
    private Button _backButton = null!;
    private Button _endTurnButton = null!;
    private SubViewportContainer _battleViewportContainer = null!;
    private SubViewport _battleViewport = null!;
    private PanelContainer _battleViewportFrame = null!;
    private Node3D _battleWorld = null!;
    private Node3D _enemyRoot = null!;
    private PanelContainer _partyPanel = null!;
    private PackedScene _cardViewScene = null!;
    private PackedScene _enemyViewScene = null!;
    private TargetingArrow _targetingArrow = null!;
    private Vector2 _dragOffset;
    private bool _isDragging;

    public override void _Ready()
    {
        _cardViewScene = ResourceLoader.Load<PackedScene>("res://scenes/card_view.tscn");
        _enemyViewScene = ResourceLoader.Load<PackedScene>("res://scenes/enemy_view.tscn");

        BuildOverlay();
        BuildWorld();
        BootBattle();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMSizeChanged && _handRoot != null)
        {
            ApplyResponsiveLayout();
            UpdateBattleViewportSize();
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
        _battleWorld = new Node3D { Name = "BattleWorld" };
        _battleViewport.AddChild(_battleWorld);

        _camera = new Camera3D
        {
            Name = "Camera3D",
            Position = new Vector3(0, 4.55f, 7.35f),
            RotationDegrees = new Vector3(-29.0f, 0, 0),
            Current = true,
            Fov = 38.0f
        };
        _battleWorld.AddChild(_camera);

        var light = new DirectionalLight3D
        {
            Name = "KeyLight",
            RotationDegrees = new Vector3(-55.0f, -28.0f, 0),
            LightEnergy = 2.2f
        };
        _battleWorld.AddChild(light);

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
        _battleWorld.AddChild(floor);

        _enemyRoot = new Node3D
        {
            Name = "EnemyRoot",
            Position = new Vector3(0, 0, -1.8f)
        };
        _battleWorld.AddChild(_enemyRoot);
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

        _topBar = new HBoxContainer
        {
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 0
        };
        overlay.AddChild(_topBar);

        _backButton = new Button
        {
            Text = "Map"
        };
        _backButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/map_scene.tscn");
        _topBar.AddChild(_backButton);

        _endTurnButton = new Button
        {
            Text = "End Turn"
        };
        _endTurnButton.Pressed += () => _runtime.EndTurn();
        _topBar.AddChild(_endTurnButton);

        _battleStatus = new Label
        {
            Text = "Starting battle...",
            VerticalAlignment = VerticalAlignment.Center,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        _topBar.AddChild(_battleStatus);

        _battleViewportContainer = new SubViewportContainer
        {
            Name = "BattleViewport",
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 1,
            MouseFilter = Control.MouseFilterEnum.Ignore,
            Stretch = false
        };
        overlay.AddChild(_battleViewportContainer);

        _battleViewport = new SubViewport
        {
            Name = "BattleSubViewport",
            RenderTargetUpdateMode = SubViewport.UpdateMode.Always,
            TransparentBg = false,
            OwnWorld3D = true,
            Size = new Vector2I(768, 360)
        };
        _battleViewportContainer.AddChild(_battleViewport);
        _battleViewportContainer.Resized += UpdateBattleViewportSize;

        _battleViewportFrame = new PanelContainer
        {
            Name = "BattleViewportFrame",
            AnchorLeft = _battleViewportContainer.AnchorLeft,
            AnchorTop = _battleViewportContainer.AnchorTop,
            AnchorRight = _battleViewportContainer.AnchorRight,
            AnchorBottom = _battleViewportContainer.AnchorBottom,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        overlay.AddChild(_battleViewportFrame);

        BuildPartyPanel(overlay);

        _handRoot = new Control
        {
            Name = "HandRoot",
            AnchorLeft = 0,
            AnchorTop = 1,
            AnchorRight = 1,
            AnchorBottom = 1,
            OffsetLeft = 0,
            OffsetRight = 0,
            OffsetBottom = 0,
            ClipContents = false
        };
        overlay.AddChild(_handRoot);

        _targetingArrow = new TargetingArrow { Name = "TargetingArrow" };
        overlay.AddChild(_targetingArrow);

        ApplyResponsiveLayout();
    }

    private void BuildPartyPanel(Control overlay)
    {
        _partyPanel = new PanelContainer
        {
            Name = "PartyPanel",
            AnchorLeft = 1,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 1
        };
        overlay.AddChild(_partyPanel);

        _partySlots = new GridContainer
        {
            Name = "PartySlots",
            Columns = 2,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };
        _partyPanel.AddChild(_partySlots);

        for (int i = 0; i < 4; i++)
        {
            var slot = new PartySlotView { Name = $"PartySlot{i + 1}" };
            slot.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            slot.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            _partySlots.AddChild(slot);
        }
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
        RefreshParty();
        RefreshHand();
    }

    private void RefreshStatus()
    {
        if (_battleStatus == null || _runtime == null)
        {
            return;
        }

        _battleStatus.Text = $"Enemies: {_runtime.GetEnemyUnits().Count}  |  Party: {_runtime.GetPartyMembers().Count}/4  |  Hand: {_runtime.GetHandCards().Count}";
    }

    private void RefreshParty()
    {
        if (_partySlots == null || _runtime == null)
        {
            return;
        }

        IReadOnlyList<IEntity> members = _runtime.GetPartyMembers();
        for (int i = 0; i < _partySlots.GetChildCount(); i++)
        {
            var slot = _partySlots.GetChild<PartySlotView>(i);
            slot.Bind(i < members.Count ? members[i] : null, i);
        }
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
            cardView.ApplyPresentationScale(GetUiScale());
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
        float uiScale = GetUiScale();
        float cardWidth = CardView.BaseWidth;
        float cardHeight = CardView.BaseHeight;
        float baseSpacing = 132.0f * uiScale;
        float usableWidth = Mathf.Max(320.0f * uiScale, viewportSize.X - 180.0f * uiScale);
        float unscaledFanWidth = (cardWidth + 132.0f * Mathf.Max(0, _cardViews.Count - 1)) * uiScale;
        float fitScale = Mathf.Clamp(usableWidth / unscaledFanWidth, 0.68f, 1.0f);
        float baseScale = uiScale * fitScale;
        float spacing = _cardViews.Count <= 1
            ? 0.0f
            : baseSpacing * fitScale;
        float scaledCardWidth = cardWidth * baseScale;
        float scaledCardHeight = cardHeight * baseScale;
        float startX = viewportSize.X * 0.5f - spacing * (_cardViews.Count - 1) * 0.5f - scaledCardWidth * 0.5f;
        float baseY = _handRoot.Size.Y - scaledCardHeight - HandBottomMargin * uiScale;
        float center = (_cardViews.Count - 1) * 0.5f;

        for (int i = 0; i < _cardViews.Count; i++)
        {
            CardView cardView = _cardViews[i];
            float offsetFromCenter = i - center;
            float normalized = _cardViews.Count <= 1 ? 0.0f : offsetFromCenter / center;
            float rotation = normalized * 9.0f;
            float y = baseY + Mathf.Abs(normalized) * 34.0f * uiScale;
            var targetPosition = new Vector2(startX + spacing * i, y);

            _cardPositions[cardView] = targetPosition;
            _cardRotations[cardView] = rotation;
            _cardScales[cardView] = baseScale;

            cardView.ZIndex = i;
            if (cardView != _draggedCard)
            {
                cardView.ApplyPresentationScale(baseScale);
                AnimateCard(cardView, targetPosition, rotation, 0.18f);
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
        cardView.ApplyPresentationScale(focusScale);
        AnimateCard(cardView, focusPosition, 0.0f, 0.13f);
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
        cardView.ApplyPresentationScale(baseScale);
        AnimateCard(cardView, position, rotation, 0.12f);
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
            float dragScale = Mathf.Min(GetUiScale() * 1.28f, baseScale * 1.14f);
            cardView.ApplyPresentationScale(dragScale);
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
        if (!_battleViewportContainer.GetGlobalRect().HasPoint(mousePosition))
        {
            return null;
        }

        Vector2 viewportOrigin = _battleViewportContainer.GlobalPosition;
        Vector2 viewportScale = GetBattleViewportScale();
        EnemyView? nearest = null;
        float nearestDistance = 72.0f;

        foreach (EnemyView enemyView in _enemyViews)
        {
            Vector2 viewportPosition = _camera.UnprojectPosition(enemyView.GlobalPosition + new Vector3(0, 0.95f, 0));
            Vector2 screenPosition = viewportOrigin + viewportPosition * viewportScale;
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
        return cardTopInHand <= restingPosition.Y - PlayAboveHandThreshold * GetUiScale();
    }

    private void AnimateCard(CardView cardView, Vector2 position, float rotationDegrees, float duration)
    {
        cardView.Scale = Vector2.One;
        Tween tween = CreateTween().SetParallel(true);
        tween.TweenProperty(cardView, "position", position, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        tween.TweenProperty(cardView, "rotation_degrees", rotationDegrees, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
    }

    private Vector2 GetFocusedCardCenter(CardView cardView)
    {
        Vector2 position = GetFocusedCardPosition(cardView);
        return _handRoot.GlobalPosition + position + cardView.Size * 0.5f;
    }

    private Vector2 GetFocusedCardPosition(CardView cardView)
    {
        Vector2 position = _cardPositions.GetValueOrDefault(cardView, cardView.GlobalPosition);
        float baseScale = _cardScales.GetValueOrDefault(cardView, 1.0f);
        float focusScale = GetFocusedCardScale(cardView);
        float widthDelta = CardView.BaseWidth * (focusScale - baseScale) * 0.5f;
        float heightDelta = CardView.BaseHeight * (focusScale - baseScale) * 0.5f;
        return new Vector2(
            position.X - widthDelta,
            Mathf.Max(6.0f * GetUiScale(), position.Y - 92.0f * baseScale - heightDelta));
    }

    private float GetFocusedCardScale(CardView cardView)
    {
        float baseScale = _cardScales.GetValueOrDefault(cardView, 1.0f);
        return Mathf.Min(GetUiScale() * 1.28f, baseScale * 1.18f);
    }

    private void ApplyResponsiveLayout()
    {
        float uiScale = GetUiScale();
        float handHeight = HandReservedHeight * uiScale;
        float partyPanelWidth = GetPartyPanelWidth();

        _topBar.OffsetLeft = 18.0f * uiScale;
        _topBar.OffsetTop = 16.0f * uiScale;
        _topBar.OffsetRight = -partyPanelWidth - 34.0f * uiScale;
        _topBar.OffsetBottom = 58.0f * uiScale;
        _topBar.AddThemeConstantOverride("separation", ScaleInt(10.0f, uiScale));

        _backButton.CustomMinimumSize = new Vector2(88.0f, 38.0f) * uiScale;
        _endTurnButton.CustomMinimumSize = new Vector2(118.0f, 38.0f) * uiScale;
        _battleStatus.AddThemeFontSizeOverride("font_size", ScaleInt(16.0f, uiScale));

        _battleViewportContainer.OffsetLeft = 10.0f * uiScale;
        _battleViewportContainer.OffsetTop = 72.0f * uiScale;
        _battleViewportContainer.OffsetRight = -partyPanelWidth - 24.0f * uiScale;
        _battleViewportContainer.OffsetBottom = -handHeight + 18.0f * uiScale;

        _battleViewportFrame.OffsetLeft = _battleViewportContainer.OffsetLeft;
        _battleViewportFrame.OffsetTop = _battleViewportContainer.OffsetTop;
        _battleViewportFrame.OffsetRight = _battleViewportContainer.OffsetRight;
        _battleViewportFrame.OffsetBottom = _battleViewportContainer.OffsetBottom;
        _battleViewportFrame.AddThemeStyleboxOverride("panel", CreateViewportFrameStyle(uiScale));

        _partyPanel.OffsetLeft = -partyPanelWidth - 14.0f * uiScale;
        _partyPanel.OffsetTop = 14.0f * uiScale;
        _partyPanel.OffsetRight = -14.0f * uiScale;
        _partyPanel.OffsetBottom = -handHeight + 18.0f * uiScale;
        _partyPanel.AddThemeStyleboxOverride("panel", CreatePartyPanelStyle(uiScale));

        _partySlots.AddThemeConstantOverride("h_separation", ScaleInt(8.0f, uiScale));
        _partySlots.AddThemeConstantOverride("v_separation", ScaleInt(8.0f, uiScale));
        foreach (Node child in _partySlots.GetChildren())
        {
            if (child is PartySlotView slot)
            {
                slot.ApplyPresentationScale(uiScale);
            }
        }

        _handRoot.OffsetTop = -handHeight;

        foreach (CardView cardView in _cardViews)
        {
            cardView.ApplyPresentationScale(_cardScales.GetValueOrDefault(cardView, uiScale));
        }
    }

    private float GetUiScale()
    {
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
        float scale = Mathf.Min(viewportSize.X / ReferenceWidth, viewportSize.Y / ReferenceHeight);
        return Mathf.Clamp(scale, 0.75f, 1.65f);
    }

    private static int ScaleInt(float value, float scale)
    {
        return Mathf.Max(1, Mathf.RoundToInt(value * scale));
    }

    private static StyleBoxFlat CreateViewportFrameStyle(float scale)
    {
        int border = ScaleInt(2.0f, scale);
        int radius = ScaleInt(4.0f, scale);
        return new StyleBoxFlat
        {
            BgColor = new Color(0, 0, 0, 0),
            BorderColor = new Color(0.54f, 0.50f, 0.42f, 0.72f),
            BorderWidthBottom = border,
            BorderWidthLeft = border,
            BorderWidthRight = border,
            BorderWidthTop = border,
            CornerRadiusBottomLeft = radius,
            CornerRadiusBottomRight = radius,
            CornerRadiusTopLeft = radius,
            CornerRadiusTopRight = radius
        };
    }

    private static StyleBoxFlat CreatePartyPanelStyle(float scale)
    {
        int border = ScaleInt(2.0f, scale);
        int radius = ScaleInt(4.0f, scale);
        int margin = ScaleInt(10.0f, scale);
        return new StyleBoxFlat
        {
            BgColor = new Color(0.075f, 0.075f, 0.070f, 0.94f),
            BorderColor = new Color(0.60f, 0.52f, 0.36f),
            BorderWidthBottom = border,
            BorderWidthLeft = border,
            BorderWidthRight = border,
            BorderWidthTop = border,
            ContentMarginBottom = margin,
            ContentMarginLeft = margin,
            ContentMarginRight = margin,
            ContentMarginTop = margin,
            CornerRadiusBottomLeft = radius,
            CornerRadiusBottomRight = radius,
            CornerRadiusTopLeft = radius,
            CornerRadiusTopRight = radius
        };
    }

    private float GetCombatWidth(Vector2 viewportSize)
    {
        return viewportSize.X - GetPartyPanelWidth();
    }

    private float GetPartyPanelWidth()
    {
        return PartyPanelWidth * GetUiScale();
    }

    private void UpdateBattleViewportSize()
    {
        if (_battleViewport == null || _battleViewportContainer == null)
        {
            return;
        }

        Vector2 size = _battleViewportContainer.Size;
        if (size.X > 1 && size.Y > 1)
        {
            _battleViewport.Size = new Vector2I(Mathf.RoundToInt(size.X), Mathf.RoundToInt(size.Y));
        }
    }

    private Vector2 GetBattleViewportScale()
    {
        if (_battleViewport.Size.X <= 0 || _battleViewport.Size.Y <= 0)
        {
            return Vector2.One;
        }

        return new Vector2(
            _battleViewportContainer.Size.X / _battleViewport.Size.X,
            _battleViewportContainer.Size.Y / _battleViewport.Size.Y);
    }
}

public partial class PartySlotView : PanelContainer
{
    private const float IconSize = 28.0f;
    private const float IconAmountSize = 13.0f;
    private const string StatusIconRoot = "res://assets/status_icons";

    private VBoxContainer _rootLayout = null!;
    private Panel _portraitPanel = null!;
    private Label _portraitLabel = null!;
    private ProgressBar _healthBar = null!;
    private Label _healthText = null!;
    private HFlowContainer _statusSymbols = null!;
    private IEntity? _member;
    private float _presentationScale = 1.0f;

    public override void _Ready()
    {
        BuildLayout();
        Refresh();
    }

    public void Bind(IEntity? member, int slotIndex)
    {
        _member = member;
        if (IsInsideTree())
        {
            Refresh(slotIndex);
        }
    }

    public void ApplyPresentationScale(float scale)
    {
        _presentationScale = Mathf.Clamp(scale, 0.75f, 1.65f);
        if (_portraitLabel == null)
        {
            return;
        }

        CustomMinimumSize = new Vector2(150.0f, 118.0f) * _presentationScale;
        AddThemeStyleboxOverride("panel", CreateSlotStyle(new Color(0.12f, 0.115f, 0.095f), _presentationScale));
        _rootLayout.AddThemeConstantOverride("separation", ScaleInt(6.0f));
        _portraitLabel.AddThemeFontSizeOverride("font_size", ScaleInt(34.0f));

        float iconSize = IconSize * _presentationScale;
        _statusSymbols.OffsetLeft = 6.0f * _presentationScale;
        _statusSymbols.OffsetTop = -iconSize - 8.0f * _presentationScale;
        _statusSymbols.OffsetRight = -6.0f * _presentationScale;
        _statusSymbols.OffsetBottom = -6.0f * _presentationScale;
        _statusSymbols.AddThemeConstantOverride("h_separation", ScaleInt(4.0f));
        _statusSymbols.AddThemeConstantOverride("v_separation", ScaleInt(4.0f));

        _healthText.AddThemeFontSizeOverride("font_size", ScaleInt(12.0f));
        Refresh();
    }

    private void BuildLayout()
    {
        _rootLayout = new VBoxContainer();
        AddChild(_rootLayout);

        _portraitPanel = new Panel
        {
            ClipContents = true,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill
        };
        _rootLayout.AddChild(_portraitPanel);

        _portraitLabel = new Label
        {
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 1,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        _portraitLabel.AddThemeColorOverride("font_color", new Color(0.98f, 0.90f, 0.70f));
        _portraitPanel.AddChild(_portraitLabel);

        _statusSymbols = new HFlowContainer
        {
            AnchorLeft = 0,
            AnchorTop = 1,
            AnchorRight = 1,
            AnchorBottom = 1,
            OffsetLeft = 6,
            OffsetTop = -IconSize - 8,
            OffsetRight = -6,
            OffsetBottom = -6
        };
        _portraitPanel.AddChild(_statusSymbols);

        var healthOverlay = new Control
        {
            CustomMinimumSize = new Vector2(0, 20),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        _rootLayout.AddChild(healthOverlay);

        _healthBar = new ProgressBar
        {
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 1,
            MinValue = 0,
            MaxValue = 1,
            Value = 0,
            ShowPercentage = false,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        _healthBar.AddThemeStyleboxOverride("background", new StyleBoxFlat { BgColor = new Color(0.10f, 0.035f, 0.035f) });
        _healthBar.AddThemeStyleboxOverride("fill", new StyleBoxFlat { BgColor = new Color(0.70f, 0.12f, 0.10f) });
        healthOverlay.AddChild(_healthBar);

        _healthText = new Label
        {
            AnchorLeft = 0,
            AnchorTop = 0,
            AnchorRight = 1,
            AnchorBottom = 1,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        _healthText.AddThemeColorOverride("font_color", new Color(0.98f, 0.94f, 0.86f));
        _healthText.AddThemeColorOverride("font_shadow_color", new Color(0, 0, 0, 0.85f));
        _healthText.AddThemeConstantOverride("shadow_offset_x", 1);
        _healthText.AddThemeConstantOverride("shadow_offset_y", 1);
        healthOverlay.AddChild(_healthText);

        ApplyPresentationScale(_presentationScale);
    }

    private void Refresh(int slotIndex = 0)
    {
        if (_portraitLabel == null)
        {
            return;
        }

        if (_member == null)
        {
            _portraitPanel.Visible = false;
            _portraitLabel.Visible = false;
            _healthBar.Visible = false;
            _healthText.Visible = false;
            _statusSymbols.Visible = false;
            _portraitLabel.Text = "";
            _portraitPanel.AddThemeStyleboxOverride("panel", CreatePortraitStyle(new Color(0.08f, 0.08f, 0.075f)));
            _healthBar.Value = 0;
            _healthBar.MaxValue = 1;
            _healthText.Text = "";
            SetStatusSymbols(Array.Empty<StatusIconInfo>());
            return;
        }

        _portraitPanel.Visible = true;
        _portraitLabel.Visible = true;
        _healthBar.Visible = true;
        _healthText.Visible = true;
        _statusSymbols.Visible = true;

        string name = GetDisplayName(_member);
        _portraitLabel.Text = GetInitials(name);
        _portraitPanel.TooltipText = name;
        _portraitPanel.AddThemeStyleboxOverride("panel", CreatePortraitStyle(ColorFromText(name)));

        var health = _member.GetComponent<Health>();
        if (health == null)
        {
            _healthBar.Value = 0;
            _healthBar.MaxValue = 1;
            _healthText.Text = "HP --";
        }
        else
        {
            _healthBar.MaxValue = Mathf.Max(1, health.Max);
            _healthBar.Value = Mathf.Clamp(health.Amount, 0, health.Max);
            _healthText.Text = $"HP {health.Amount}/{health.Max}";
        }

        SetStatusSymbols(GetStatusIcons(_member));
    }

    private IEnumerable<StatusIconInfo> GetStatusIcons(IEntity member)
    {
        foreach (Component component in member.Components.OfType<Component>().Where(component => component is IStatusEffect).Take(6))
        {
            string typeName = component.GetType().Name;
            string tooltip = component is ITooltip tooltipComponent && !string.IsNullOrWhiteSpace(tooltipComponent.Tooltip)
                ? tooltipComponent.Tooltip
                : typeName;

            yield return new StatusIconInfo(
                GetStatusFallbackText(typeName),
                tooltip,
                ColorFromText(typeName).Lightened(0.25f),
                LoadStatusIconTexture(typeName),
                component is IAmount amount ? amount.Amount : null);
        }
    }

    private void SetStatusSymbols(IEnumerable<StatusIconInfo> symbols)
    {
        foreach (Node child in _statusSymbols.GetChildren())
        {
            child.QueueFree();
        }

        foreach (StatusIconInfo symbol in symbols.DistinctBy(symbol => symbol.Tooltip).Take(6))
        {
            float iconSize = IconSize * _presentationScale;
            float iconAmountSize = IconAmountSize * _presentationScale;
            var icon = new Control
            {
                CustomMinimumSize = new Vector2(iconSize, iconSize),
                ClipContents = false,
                TooltipText = symbol.Tooltip
            };

            var iconBackground = new Panel
            {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 1,
                AnchorBottom = 1,
                MouseFilter = MouseFilterEnum.Ignore
            };
            iconBackground.AddThemeStyleboxOverride("panel", new StyleBoxFlat
            {
                BgColor = symbol.Color.Darkened(0.42f),
                BorderColor = symbol.Color.Lightened(0.28f),
                BorderWidthBottom = ScaleInt(1.0f),
                BorderWidthLeft = ScaleInt(1.0f),
                BorderWidthRight = ScaleInt(1.0f),
                BorderWidthTop = ScaleInt(1.0f),
                CornerRadiusBottomLeft = ScaleInt(11.0f),
                CornerRadiusBottomRight = ScaleInt(11.0f),
                CornerRadiusTopLeft = ScaleInt(11.0f),
                CornerRadiusTopRight = ScaleInt(11.0f)
            });
            icon.AddChild(iconBackground);

            if (symbol.Texture != null)
            {
                icon.AddChild(new TextureRect
                {
                    AnchorLeft = 0,
                    AnchorTop = 0,
                    AnchorRight = 1,
                    AnchorBottom = 1,
                    Texture = symbol.Texture,
                    ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional,
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    MouseFilter = MouseFilterEnum.Ignore
                });
            }
            else
            {
                var label = new Label
                {
                    Text = symbol.FallbackText,
                    AnchorLeft = 0,
                    AnchorTop = 0,
                    AnchorRight = 1,
                    AnchorBottom = 1,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    MouseFilter = MouseFilterEnum.Ignore
                };
                label.AddThemeFontSizeOverride("font_size", ScaleInt(15.0f));
                label.AddThemeColorOverride("font_color", new Color(0.98f, 0.94f, 0.84f));
                icon.AddChild(label);
            }

            if (symbol.Amount.HasValue)
            {
                var amountLabel = new Label
                {
                    Text = symbol.Amount.Value.ToString(),
                    Position = new Vector2(iconSize - iconAmountSize + 2.0f * _presentationScale, iconSize - iconAmountSize + 2.0f * _presentationScale),
                    Size = new Vector2(iconAmountSize, iconAmountSize),
                    CustomMinimumSize = new Vector2(iconAmountSize, iconAmountSize),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    MouseFilter = MouseFilterEnum.Ignore
                };
                amountLabel.AddThemeFontSizeOverride("font_size", ScaleInt(9.0f));
                amountLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.96f, 0.84f));
                amountLabel.AddThemeColorOverride("font_shadow_color", new Color(0, 0, 0, 0.90f));
                amountLabel.AddThemeConstantOverride("shadow_offset_x", 1);
                amountLabel.AddThemeConstantOverride("shadow_offset_y", 1);
                amountLabel.AddThemeStyleboxOverride("normal", new StyleBoxFlat
                {
                    BgColor = new Color(0.05f, 0.035f, 0.025f, 0.88f),
                    BorderColor = symbol.Color.Lightened(0.18f),
                    BorderWidthBottom = ScaleInt(1.0f),
                    BorderWidthLeft = ScaleInt(1.0f),
                    BorderWidthRight = ScaleInt(1.0f),
                    BorderWidthTop = ScaleInt(1.0f),
                    CornerRadiusBottomLeft = ScaleInt(6.0f),
                    CornerRadiusBottomRight = ScaleInt(6.0f),
                    CornerRadiusTopLeft = ScaleInt(6.0f),
                    CornerRadiusTopRight = ScaleInt(6.0f)
                });
                icon.AddChild(amountLabel);
            }

            _statusSymbols.AddChild(icon);
        }
    }

    private Texture2D? LoadStatusIconTexture(string typeName)
    {
        foreach (string candidate in GetStatusIconPathCandidates(typeName))
        {
            if (ResourceLoader.Exists(candidate))
            {
                return ResourceLoader.Load<Texture2D>(candidate);
            }
        }

        return null;
    }

    private IEnumerable<string> GetStatusIconPathCandidates(string typeName)
    {
        string simpleName = typeName;
        string lowerName = typeName.ToLowerInvariant();
        foreach (string extension in new[] { "png", "svg", "webp", "jpg", "jpeg" })
        {
            yield return $"{StatusIconRoot}/{simpleName}.{extension}";
            yield return $"{StatusIconRoot}/{lowerName}.{extension}";
        }
    }

    private string GetStatusFallbackText(string typeName)
    {
        return typeName switch
        {
            nameof(Weak) => "W",
            nameof(Vulnerable) => "V",
            nameof(Thorns) => "T",
            nameof(Regen) => "R",
            nameof(Frozen) => "F",
            nameof(Burn) => "B",
            nameof(Bloodied) => "!",
            nameof(Chained) => "C",
            _ => typeName.Length == 0 ? "?" : typeName[..1].ToUpperInvariant()
        };
    }

    private string GetDisplayName(IEntity member)
    {
        return member.GetComponent<PartyMember>()?.DisplayName
               ?? member.GetComponent<NameComponent>()?.Value
               ?? $"Member {member.Id}";
    }

    private string GetInitials(string name)
    {
        string[] parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return "?";
        }

        return string.Concat(parts.Take(2).Select(part => char.ToUpperInvariant(part[0])));
    }

    private StyleBoxFlat CreatePortraitStyle(Color color)
    {
        int border = ScaleInt(2.0f);
        int radius = ScaleInt(4.0f);
        return new StyleBoxFlat
        {
            BgColor = color.Darkened(0.25f),
            BorderColor = color.Lightened(0.24f),
            BorderWidthBottom = border,
            BorderWidthLeft = border,
            BorderWidthRight = border,
            BorderWidthTop = border,
            CornerRadiusBottomLeft = radius,
            CornerRadiusBottomRight = radius,
            CornerRadiusTopLeft = radius,
            CornerRadiusTopRight = radius
        };
    }

    private StyleBoxFlat CreateSlotStyle(Color color, float scale)
    {
        int border = Mathf.Max(1, Mathf.RoundToInt(1.0f * scale));
        int radius = Mathf.Max(1, Mathf.RoundToInt(3.0f * scale));
        int margin = Mathf.Max(1, Mathf.RoundToInt(8.0f * scale));
        return new StyleBoxFlat
        {
            BgColor = color,
            BorderColor = new Color(0.42f, 0.36f, 0.24f),
            BorderWidthBottom = border,
            BorderWidthLeft = border,
            BorderWidthRight = border,
            BorderWidthTop = border,
            ContentMarginBottom = margin,
            ContentMarginLeft = margin,
            ContentMarginRight = margin,
            ContentMarginTop = margin,
            CornerRadiusBottomLeft = radius,
            CornerRadiusBottomRight = radius,
            CornerRadiusTopLeft = radius,
            CornerRadiusTopRight = radius
        };
    }

    private int ScaleInt(float value)
    {
        return Mathf.Max(1, Mathf.RoundToInt(value * _presentationScale));
    }

    private Color ColorFromText(string text)
    {
        uint hash = 2166136261;
        foreach (char character in text)
        {
            hash ^= character;
            hash *= 16777619;
        }

        float hue = (hash % 360) / 360.0f;
        return Color.FromHsv(hue, 0.45f, 0.48f);
    }

    private readonly record struct StatusIconInfo(string FallbackText, string Tooltip, Color Color, Texture2D? Texture, int? Amount);
}
