using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class DeathAddsMoneyToPlayer : SummerJam1Component, ITooltip
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

        public string Tooltip => "Drops money when destroyed.";
    }
    
    

    public class DeathAddsRandomCardsToPrizePile : SummerJam1Component, IDescription
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Game.PrizePile.SetupRandomPrizePile();
            }
        }

        public string Description => "Destroy to gain a choice of cards.";
    }
}
