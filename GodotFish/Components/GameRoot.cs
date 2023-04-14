using Api;
using Godot;
using SummerJam1;

public partial class GameRoot : Node
{
    private Api.Context Context { get; set; }
    public override void _Ready()
    {
        base._Ready();
        Context = new Context(new SummerJam1Events());
        Context.CreateEntity(null, Setup);
    }

    private void Setup(IEntity newchild)
    {
        newchild.AddComponent<Game>();
    }
}