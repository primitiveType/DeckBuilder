using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class Retain : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Retain - This card is not discarded at end of turn.";
    }
}