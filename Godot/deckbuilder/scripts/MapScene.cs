using Godot;

namespace Deckbuilder;

public partial class MapScene : Control
{
    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        var root = new VBoxContainer
        {
            AnchorLeft = 0.5f,
            AnchorTop = 0.5f,
            AnchorRight = 0.5f,
            AnchorBottom = 0.5f,
            OffsetLeft = -220,
            OffsetTop = -120,
            OffsetRight = 220,
            OffsetBottom = 120
        };
        root.AddThemeConstantOverride("separation", 18);
        AddChild(root);

        var title = new Label
        {
            Text = "Map",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        title.AddThemeFontSizeOverride("font_size", 34);
        root.AddChild(title);

        var startBattleButton = new Button
        {
            Text = "Start Battle",
            CustomMinimumSize = new Vector2(260, 48)
        };
        startBattleButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/battle_scene.tscn");
        root.AddChild(startBattleButton);
    }
}
