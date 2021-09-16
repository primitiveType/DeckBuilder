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
            var playerCoord = new AxialHexCoord(0, 0).ToCubic();
            Graph.Nodes[playerCoord].TryAdd(player);

            var enemyCoord = playerCoord.Neighbor(DirectionEnum.E);
            Graph.Nodes[enemyCoord].TryAdd(enemy);
            var enemyCoord2 = enemyCoord.Neighbor(DirectionEnum.E);
            Graph.Nodes[enemyCoord2].TryAdd(enemy2);
        }
    }
}