using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class AddsVulnerableOnHit: AddsComponentOnHit<Vulnerable>, ITooltip
    {
        public string Tooltip => Tooltips.VULN_TOOLTIP;
    }
}