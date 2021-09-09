using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class FiveSlotBattleData : BattleData<SquareWithCenterPointGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
            // BasicEnemy enemy2 = Context.CreateActor<BasicEnemy>(100, 0);
            BasicEnemy enemy3 = Context.CreateActor<BasicEnemy>(100, 0);
            BasicEnemy enemy4 = Context.CreateActor<BasicEnemy>(100, 0);//should add a ranged enemy

            Graph.TopLeft.SetActorNoEvent(player);
            Graph.Middle.SetActorNoEvent(enemy);
            // Graph.TopRight.SetActorNoEvent(enemy2);
            Graph.BottomLeft.SetActorNoEvent(enemy3);
            Graph.BottomRight.SetActorNoEvent(enemy4);
        }
    }
}