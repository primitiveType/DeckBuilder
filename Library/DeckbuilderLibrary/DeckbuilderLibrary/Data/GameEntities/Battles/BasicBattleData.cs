using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class BasicBattleData : BattleData<ThreeColumnGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);

            Graph.Left.SetActorNoEvent(player);
            Graph.Right.SetActorNoEvent(enemy);
        }
    }
}