using Api;

namespace CardsAndPiles.Components
{
    public interface IDraggable : IComponent
    {
        bool CanDrag { get; }
    }
}
