using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles.TestBattles
{
    public class TestBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
    
            Graph.Nodes[new AxialHexCoord(0, 1).ToCubic()].TryAdd(enemy);
            Graph.Nodes[new AxialHexCoord(0, 0).ToCubic()].TryAdd(player);
            // Graph.Left.AddEntityNoEvent(player);
            // Graph.Middle.AddEntityNoEvent(enemy);
        }
    }
}