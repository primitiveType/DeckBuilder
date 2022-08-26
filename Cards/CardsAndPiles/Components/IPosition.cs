using System.Numerics;
using Api;

namespace CardsAndPiles.Components
{
    public interface IPosition : IComponent
    {
        Vector3 Pos { get; set; }
    }
}
