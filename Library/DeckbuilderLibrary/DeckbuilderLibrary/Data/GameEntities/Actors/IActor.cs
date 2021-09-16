using ca.axoninteractive.Geometry.Hex;

namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public interface IActor : IGameEntity, ICoordinateProperty
    {
        int Health { get; }
        int Armor { get; }
        Resources.Resources Resources { get; }
    }

    public interface ICoordinateProperty
    {
        CubicHexCoord Coordinate { get; }
    }
    
    public interface IInternalCoordinateProperty
    {
        CubicHexCoord Coordinate { get; set; }
    }
   
}