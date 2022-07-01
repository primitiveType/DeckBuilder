using System.Numerics;
using Component = Api.Component;
using IComponent = Api.IComponent;

namespace CardsAndPiles.Components
{
    public class Draggable : Component, IDraggable
    {
        public bool CanDrag { get; set; }
    }

    public class Position : Component
    {
        public Vector3 Position1 { get; set; }
    }

    public interface IDraggable : IComponent
    {
        bool CanDrag { get; }
    }
}
