using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class TwoEnemyBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {

            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
            BasicEnemy enemy2 = Context.CreateActor<BasicEnemy>(100, 0);
            
            
            Graph.Nodes[new AxialHexCoord(0, 2).ToCubic()].TryAdd(enemy);
            Graph.Nodes[new AxialHexCoord(0, 1).ToCubic()].TryAdd(enemy2);
            Graph.Nodes[new AxialHexCoord(0, 0).ToCubic()].TryAdd(player);

            // Graph.Left.AddEntityNoEvent(player);
            // Graph.Middle.AddEntityNoEvent(enemy);
            // Graph.Right.AddEntityNoEvent(enemy2);
        }
    }
}