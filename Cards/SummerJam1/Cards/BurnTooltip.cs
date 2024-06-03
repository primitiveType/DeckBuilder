using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class BurnTooltip : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Burn - At end of turn deals damage equal to the amount of Burn, then reduces Burn by 1.";
    }
}