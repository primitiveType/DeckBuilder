using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Relics
{
    public class ReachingXHealthHealsForY : SummerJam1Component, ITooltip, IDescription
    {
        public int TargetHealth { get; set; }
        public int HealAmount { get; set; }
        public string Description => Tooltip;
        public string Tooltip => $"When your health reaches exactly {TargetHealth}, heal for {HealAmount}.";
    }
    
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