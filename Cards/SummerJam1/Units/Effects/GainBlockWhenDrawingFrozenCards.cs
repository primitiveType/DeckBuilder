using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public class GainBlockWhenDrawingFrozenCards : EnabledWhenAtTopOfEncounterSlot, ITooltip, IDescription, IAmount
    {
        public int Amount { get; set; }
        public string Description => Tooltip;
        public string Tooltip => $"Gains {Amount} block whenever you draw a frozen card.";

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.DrawnCard.HasComponent<Frozen>())
            {
                Entity.GetOrAddComponent<Armor>().Amount += Amount;
            }
        }
    }
}