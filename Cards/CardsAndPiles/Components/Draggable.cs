using System.Numerics;
using Component = Api.Component;
using IComponent = Api.IComponent;

namespace CardsAndPiles.Components
{
    public class Draggable : Component, IDraggable
    {
        public bool CanDrag { get; set; }
    }

    public class Position : Component, IPosition
    {
        public Vector3 Pos { get; set; }
    }

    public interface IPosition : IComponent
    {
        Vector3 Pos { get; set; }
    }

    public interface IDraggable : IComponent
    {
        bool CanDrag { get; }
    }
}
