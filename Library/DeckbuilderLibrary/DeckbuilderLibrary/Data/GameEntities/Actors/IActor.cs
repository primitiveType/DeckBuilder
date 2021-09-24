using ca.axoninteractive.Geometry.Hex;

namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public interface IActor : IGameEntity, ICoordinateProperty, IBlocksMovement
    {
        int Health { get; }
        int Armor { get; }
        Resources.Resources Resources { get; }

        ActorNode Node { get; }
    }

    public interface IBlocksMovement : IInternalCoordinateProperty
    {
    }

    public interface ICoordinateProperty : IGameEntity
    {
        CubicHexCoord Coordinate { get; }
    }

    public interface IInternalCoordinateProperty : ICoordinateProperty
    {
        CubicHexCoord Coordinate { get; set; }
    }
}