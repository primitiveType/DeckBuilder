using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class AddsWeakOnHit : AddsComponentOnHit<Weak>
    {
        public string Tooltip => Tooltips.WEAK_TOOLTIP;
    }
}