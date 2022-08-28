using CardsAndPiles;

namespace SummerJam1
{
    public class DeathAddsMoneyToPlayer : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                int amount = Entity.GetComponent<Money>().Amount;
                Game.Player.Entity.GetOrAddComponent<Money>().Amount += amount;
            }
        }
    }
    
    

    public class DeathAddsRandomCardsToPrizePile : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Game.PrizePile.SetupRandomPrizePile();
            }
        }
    }
}
