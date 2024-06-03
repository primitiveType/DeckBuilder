using CardsAndPiles;

namespace SummerJam1.Units
{
    public class BattleEndsWhenDefeated : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Events.OnBattleEnded(new BattleEndedEventArgs(true));
            }
        }
    }
}