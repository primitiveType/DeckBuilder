using Component = Api.Component;
using IComponent = Api.IComponent;

namespace CardsAndPiles.Components
{
    public class Draggable : Component, IDraggable
    {
        public bool CanDrag { get; set; }
    }

    public interface IDraggable : IComponent
    {
        bool CanDrag { get; }
    }
}