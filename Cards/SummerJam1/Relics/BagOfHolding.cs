using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Relics
{
    public class BagOfHolding : SummerJam1Component, ITooltip, IDescription
    {
        public BagOfHoldingHandPile Pile { get; set; }
        public string Description => Tooltip;
        public string Tooltip => $"Stores cards to be played on later turns.";

        protected override void Initialize()
        {
            base.Initialize();
            Pile = Context.CreateEntity<BagOfHoldingHandPile>(Entity);
        }
    }
}