using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles.TestBattles
{
    public class TestBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);

            if (Graph.TryGetNode(new AxialHexCoord(0, 1).ToCubic(), out var node1))
            {
                node1.TryAdd(enemy);
            }

            if (Graph.TryGetNode(new AxialHexCoord(0, 0).ToCubic(), out var node2))
            {
                node2.TryAdd(player);
            }
            // Graph.Left.AddEntityNoEvent(player);
            // Graph.Middle.AddEntityNoEvent(enemy);
        }
    }
}