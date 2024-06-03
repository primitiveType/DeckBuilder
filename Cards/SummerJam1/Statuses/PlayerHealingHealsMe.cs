using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class InvulnerableOnEvenBeats : SummerJam1Component, IStatusEffect, ITooltip
    {
        public string Tooltip => "Immune to damage on even numbered beats.";

        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (Game.Battle.BeatTracker.CurrentBeat % 2 == 1)//this looks wrong, but is correct because beat 0 is displayed as beat 1 to player
            {
                args.Multiplier.Add(0);
            }
        }
    }
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