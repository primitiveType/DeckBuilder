using ca.axoninteractive.Geometry.Hex;
using SCGraphTheory.AdjacencyList;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public abstract class BattleGraph : GameEntity
    {
        internal HexGrid Graph { get; } = new HexGrid(.5f);

    
    }
}