namespace Api;

public class EventHandle : IDisposable
{
    public void Dispose()
    {
    }
}
public sealed class Entity
{
    public int Id { get; init; }
    public HashSet<Component> Components { get; } = new HashSet<Component>();

    private List<EventHandle> Handles { get; } = new List<EventHandle>();

    internal void Initialize()//game context paramater?
    {
        //call generated code that does reflection to get all event attributes and subscribes to proper events
        //should also iterate components? OR should it only happen in components? depends on whether this stays sealed.
    }

    internal void Terminate()
    {
        //dispose event handles we created from initialize.
    }
}

public class Component
{
    
}

public class Card : Component
{
    
}