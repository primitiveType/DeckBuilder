using Godot;

namespace Deckbuilder;

public partial class MainMenu : Control
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
            OffsetLeft = -180,
            OffsetTop = -110,
            OffsetRight = 180,
            OffsetBottom = 110
        };
        root.AddThemeConstantOverride("separation", 18);
        AddChild(root);

        var title = new Label
        {
            Text = "Deckbuilder",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        title.AddThemeFontSizeOverride("font_size", 38);
        root.AddChild(title);

        var newGameButton = new Button
        {
            Text = "New Game",
            CustomMinimumSize = new Vector2(240, 48)
        };
        newGameButton.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/map_scene.tscn");
        root.AddChild(newGameButton);
    }
}
