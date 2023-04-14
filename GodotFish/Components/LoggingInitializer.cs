using Api;
using Godot;

public partial class LoggingInitializer : Node
{
    public override void _Ready()
    {
        base._Ready();
        Logging.Initialize(new GodotLogger());
    }
}