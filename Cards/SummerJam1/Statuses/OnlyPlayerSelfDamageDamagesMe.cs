using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class OnlyPlayerSelfDamageDamagesMe : SummerJam1Component, IStatusEffect, ITooltip
    {
        public string Tooltip => "Unnatural - this creature only takes damage when you damage yourself.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity && args.SourceEntityId != Entity)
            {
                Entity.GetComponent<Health>().TryDealDamage(args.Amount, Entity);
            }
        }

        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Target == Entity && args.Source != Entity)
            {
                args.Multiplier.Add(0);
            }
        }
    }
}
