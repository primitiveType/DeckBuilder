using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class PierceTooltip : SummerJam1Component, ITooltip
    {
        public const string PIERCE_TOOLTIP = "Pierce - Hit all face-up enemies in the stack.";
        public string Tooltip => PIERCE_TOOLTIP;
    }
}
