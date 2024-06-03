using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class StrengthTooltip : SummerJam1Component, ITooltip
    {
        public const string STRENGTH_TOOLTIP = "Strength - Increases damage dealt.";
        public string Tooltip => STRENGTH_TOOLTIP;
    }
}
