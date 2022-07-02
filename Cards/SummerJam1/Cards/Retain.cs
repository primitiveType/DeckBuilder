using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class Retain : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Retain - This card is not discarded at end of turn.";
    }

    public class StrengthTooltip : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Strength- Increases damage dealt by units.";
    }

    public class BurnTooltip : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Burn - At end of turn deals damage equal to the amount of Burn, then reduces Burn by 1.";
    }

    public class RegenTooltip : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Regen - At end of turn heals unit equal to the amount of Regen, then reduces Regen by 1.";
    }
}
