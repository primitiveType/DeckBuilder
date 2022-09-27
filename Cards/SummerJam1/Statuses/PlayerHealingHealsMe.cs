using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class PlayerHealingHealsMe : EnabledWhenAtTopOfEncounterSlot, IStatusEffect, ITooltip
    {
        public string Tooltip => "Whenever the player heals, this unit heals for the same amount.";

        [OnHealDealt]
        private void OnHealDealt(object sender, HealDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity)
            {
                Entity.GetComponent<Health>().TryHeal(args.Amount, Entity);
            }
        }
    }
}