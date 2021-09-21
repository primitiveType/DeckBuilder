using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Extensions
{
    public static class HexExtensions
    {
        public static ActorNode ToActorNode(this CubicHexCoord coord, IContext context)
        {
            if (context.GetCurrentBattle().Graph.TryGetNode(coord, out var node))
            {
                return node;
            }

            return null;
        }
    }

}