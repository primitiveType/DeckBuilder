using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class Thorns: SummerJam1Component, IStatusEffect, ITooltip, IAmount
    {
        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.EntityId == Entity)
            {
                var damager = args.SourceEntityId.GetComponent<ITakesDamage>();
                if (damager == null)
                {
                    Logging.LogWarning("No way to deal damage from thorns!");
                    return;
                }

                damager.TryDealDamage(Amount, Entity);
            }
        }
        public string Tooltip => $"{Tooltips.THORNS_TOOLTIP} ({Amount})";
        public int Amount { get; set; } = 1;
    }
}