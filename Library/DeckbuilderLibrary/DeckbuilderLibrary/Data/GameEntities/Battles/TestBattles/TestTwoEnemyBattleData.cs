using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Actors.Test;

namespace DeckbuilderLibrary.Data.GameEntities.Battles.TestBattles
{
    public class TestTwoEnemyBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            TestEnemyNoMovement enemy = Context.CreateActor<TestEnemyNoMovement>(100, 0);
            TestEnemyNoMovement enemy2 = Context.CreateActor<TestEnemyNoMovement>(100, 0);
            var playerCoord = new AxialHexCoord(0, 0).ToCubic();
            Graph.Nodes[playerCoord].TryAdd(player);

            var enemyCoord = playerCoord.Neighbor(DirectionEnum.E);
            Graph.Nodes[enemyCoord].TryAdd(enemy);
            var enemyCoord2 = enemyCoord.Neighbor(DirectionEnum.E);
            Graph.Nodes[enemyCoord2].TryAdd(enemy2);
        }
    }
}