using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class RegenTooltip : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Regen - At end of turn heals unit equal to the amount of Regen, then reduces Regen by 1.";
    }
}