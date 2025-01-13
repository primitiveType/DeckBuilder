using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class Weak: SummerJam1Component, IStatusEffect, ITooltip, IAmount
    {
        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Source == Entity)
            {
                args.Multiplier.Add(-0.25f);
            }
        }
        public string Tooltip => $"{Tooltips.WEAK_TOOLTIP} for {Amount} turns";
        public int Amount { get; set; }
    }
}