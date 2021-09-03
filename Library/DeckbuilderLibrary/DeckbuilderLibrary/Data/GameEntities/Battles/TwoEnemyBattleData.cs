using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class TwoEnemyBattleData : BattleData<ThreeColumnGraph>
    {
        public override void PrepareBattle(Actor player)
        {

            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
            BasicEnemy enemy2 = Context.CreateActor<BasicEnemy>(100, 0);

            Graph.Left.SetActorNoEvent(player);
            Graph.Middle.SetActorNoEvent(enemy);
            Graph.Right.SetActorNoEvent(enemy2);
        }
    }
}