namespace SummerJam1.Cards
{
    public class DrawOnFirstTurn : SummerJam1Component
    {
        [OnBattleStarted]
        private void BattleStarted()
        {
            if (Entity.GetComponentInParent<BattleContainer>() != null)
            {
                Entity.TrySetParent(Game.Battle.Hand.Entity);
            }
        }
    }
}