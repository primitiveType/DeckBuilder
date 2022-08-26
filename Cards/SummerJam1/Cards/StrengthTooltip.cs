using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class StrengthTooltip : SummerJam1Component, ITooltip
    {
        public static string s_Tooltip = "Strength - Increases damage dealt.";
        public string Tooltip => s_Tooltip;
    }
}
