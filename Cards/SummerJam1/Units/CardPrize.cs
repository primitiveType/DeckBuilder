using CardsAndPiles;

namespace SummerJam1.Units
{
    public class CardPrize : SummerJam1Component
    {
        public string Prefab { get; set; }

        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                Game.PrizePile.AddPrefab(Prefab);
            }
        }
    }

    public class GameEndsWhenDefeated : SummerJam1Component
    {
        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                Events.OnGameEnded(new GameEndedEventArgs(true));
            }
        }
    }

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
    
    public class CollectableIntoDeck : SummerJam1Component{}
}
