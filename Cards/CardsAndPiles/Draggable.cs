using Component = Api.Component;
using IComponent = Api.IComponent;

namespace CardsAndPiles
{
    public class Draggable : Component, IDraggable
    {
        public bool CanDrag { get; set; }
    }

    public interface IDraggable : IComponent
    {
        bool CanDrag { get; set; }
    }
}