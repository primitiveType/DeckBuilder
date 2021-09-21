using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Actors.Test;

namespace DeckbuilderLibrary.Data.GameEntities.Battles.TestBattles
{
    public class TestTwoEnemyBattleData : BattleData<HexGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            DummyEnemy enemy = Context.CreateActor<DummyEnemy>(100, 0);
            DummyEnemy enemy2 = Context.CreateActor<DummyEnemy>(100, 0);
            var playerCoord = new AxialHexCoord(0, 0).ToCubic();


            Graph.GetNodes()[playerCoord].TryAdd(player);

            var enemyCoord = playerCoord.Neighbor(DirectionEnum.E);
            Graph.GetNodes()[enemyCoord].TryAdd(enemy);
            var enemyCoord2 = enemyCoord.Neighbor(DirectionEnum.E);
            Graph.GetNodes()[enemyCoord2].TryAdd(enemy2);
        }
    }
}