using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles.TestBattles
{
    public class TestBattleData : BattleData<ThreeColumnGraph>
    {
        public override void PrepareBattle(Actor player)
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);

            Graph.Left.SetActorNoEvent(player);
            Graph.Middle.SetActorNoEvent(enemy);
        }
    }
}