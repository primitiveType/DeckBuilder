using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Units.Effects
{
    public class DeathAddsMoneyToPlayer : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Drops money when destroyed.";

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
}