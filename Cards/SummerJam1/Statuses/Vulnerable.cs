using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class Vulnerable: SummerJam1Component, IStatusEffect, ITooltip, IAmount
    {
        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Target == Entity)
            {
                args.Multiplier.Add(0.5f);
            }
        }
        public string Tooltip => $"{Tooltips.VULN_TOOLTIP} for {Amount} turns";
        public int Amount { get; set; } = 1;
    }
}