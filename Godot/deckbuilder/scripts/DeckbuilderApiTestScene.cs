using System;
using Api;
using Godot;

namespace Deckbuilder;

public partial class DeckbuilderApiTestScene : Control
{
    private const string DefaultContentRoot = "res://../../Cards/SummerJam1/StreamingAssets";

    [Export]
    public bool AutoBoot { get; set; } = true;

    [Export]
    public bool AutoStartBattle { get; set; } = true;

    private GameRuntime _runtime = null!;
    private Label _statusLabel = null!;
    private RichTextLabel _log = null!;
    private Button _bootButton = null!;
    private Button _battleButton = null!;
    private Button _turnButton = null!;
    private Button _waitButton = null!;
    private Button _smokeButton = null!;
    private HFlowContainer _handCards = null!;
    private Label _handHeader = null!;
    private PackedScene _cardViewScene = null!;

    public override void _Ready()
    {
        _runtime = GetNode<GameRuntime>("GameRuntime");
        _cardViewScene = ResourceLoader.Load<PackedScene>("res://scenes/card_view.tscn");
        BuildUi();
        WireEvents();
        RefreshSnapshot();
        RefreshHand();

        if (AutoBoot)
        {
            CallDeferred(nameof(BootDefaultRuntime));
        }
    }

    public void BootDefaultRuntime()
    {
        RunApiAction(() =>
        {
            BootRuntime();
            if (AutoStartBattle)
            {
                _runtime.StartBattle();
            }
        });
    }

    private void BuildUi()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        var root = new VBoxContainer
        {
            Name = "RootLayout",
            OffsetLeft = 24,
            OffsetTop = 24,
            OffsetRight = -24,
            OffsetBottom = -24
        };
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.AddThemeConstantOverride("separation", 12);
        AddChild(root);

        var title = new Label { Text = "Deckbuilder API Test" };
        title.AddThemeFontSizeOverride("font_size", 28);
        root.AddChild(title);

        _statusLabel = new Label
        {
            Text = "Runtime not booted.",
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
        _statusLabel.AddThemeFontSizeOverride("font_size", 16);
        root.AddChild(_statusLabel);

        var toolbar = new HFlowContainer { Name = "Toolbar" };
        toolbar.AddThemeConstantOverride("h_separation", 8);
        toolbar.AddThemeConstantOverride("v_separation", 8);
        root.AddChild(toolbar);

        _bootButton = AddButton(toolbar, "Boot Runtime");
        _battleButton = AddButton(toolbar, "Start Battle");
        _turnButton = AddButton(toolbar, "End Turn");
        _waitButton = AddButton(toolbar, "Wait For Card");
        _smokeButton = AddButton(toolbar, "Create Entity");

        var content = new HSplitContainer
        {
            Name = "Content",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(content);

        var handPanel = new PanelContainer
        {
            Name = "HandPanel",
            CustomMinimumSize = new Vector2(460, 0),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        content.AddChild(handPanel);

        var handLayout = new VBoxContainer();
        handLayout.AddThemeConstantOverride("separation", 8);
        handPanel.AddChild(handLayout);

        _handHeader = new Label { Text = "Hand" };
        _handHeader.AddThemeFontSizeOverride("font_size", 18);
        handLayout.AddChild(_handHeader);

        var handScroll = new ScrollContainer
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        handLayout.AddChild(handScroll);

        _handCards = new HFlowContainer
        {
            Name = "HandCards",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        _handCards.AddThemeConstantOverride("h_separation", 10);
        _handCards.AddThemeConstantOverride("v_separation", 10);
        handScroll.AddChild(_handCards);

        _log = new RichTextLabel
        {
            Name = "EventLog",
            FitContent = false,
            ScrollFollowing = true,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            Text = ""
        };
        _log.AddThemeFontSizeOverride("normal_font_size", 14);
        content.AddChild(_log);
    }

    private Button AddButton(Control parent, string text)
    {
        var button = new Button
        {
            Text = text,
            CustomMinimumSize = new Vector2(132, 40)
        };
        parent.AddChild(button);
        return button;
    }

    private void WireEvents()
    {
        _runtime.MessageLogged += AppendLog;
        _runtime.SnapshotChanged += RefreshSnapshot;
        _runtime.HandChanged += RefreshHand;

        _bootButton.Pressed += OnBootPressed;
        _battleButton.Pressed += () => RunApiAction(_runtime.StartBattle);
        _turnButton.Pressed += () => RunApiAction(_runtime.EndTurn);
        _waitButton.Pressed += () => RunApiAction(_runtime.WaitForCard);
        _smokeButton.Pressed += () => RunApiAction(_runtime.CreateSmokeEntity);
    }

    private void OnBootPressed()
    {
        RunApiAction(BootRuntime);
    }

    private void BootRuntime()
    {
        string contentRoot = ProjectSettings.GlobalizePath(DefaultContentRoot);
        _runtime.Boot(contentRoot);
    }

    private void RunApiAction(Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            AppendLog($"ERROR: {exception.Message}");
            GD.PushError(exception.ToString());
        }
        finally
        {
            RefreshSnapshot();
        }
    }

    private void RefreshSnapshot()
    {
        _statusLabel.Text = _runtime.GetSnapshot();
        bool booted = _runtime.IsBooted;
        _battleButton.Disabled = !booted;
        _turnButton.Disabled = !booted;
        _waitButton.Disabled = !booted;
        _smokeButton.Disabled = !booted;
        RefreshHand();
    }

    private void AppendLog(string message)
    {
        _log.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
    }

    private void RefreshHand()
    {
        if (_handCards == null)
        {
            return;
        }

        foreach (Node child in _handCards.GetChildren())
        {
            child.Free();
        }

        var cards = _runtime.GetHandCards();
        _handHeader.Text = cards.Count == 1 ? "Hand (1 card)" : $"Hand ({cards.Count} cards)";

        if (cards.Count == 0)
        {
            var emptyLabel = new Label
            {
                Text = _runtime.Game?.Battle == null ? "No active battle." : "No cards in hand.",
                CustomMinimumSize = new Vector2(220, 40)
            };
            _handCards.AddChild(emptyLabel);
            return;
        }

        foreach (IEntity card in cards)
        {
            var cardView = _cardViewScene.Instantiate<CardView>();
            cardView.Bind(card);
            _handCards.AddChild(cardView);
        }
    }
}
