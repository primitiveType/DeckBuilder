using SummerJam1;

namespace CardsAndPiles.Components
{
    public class Power : SummerJam1Component
    {
        protected bool PowerActive => Entity.Parent == Game.Player.Entity;
        
        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            Entity.Destroy();
        }
        
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                Entity.TrySetParent(Game.Player.Entity);
            }
        }
    }
}