using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class Burn : SummerJam1Component, IAmount
    {
        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetComponent<Health>().TryDealDamage(Amount, Entity);
            Amount--;
            if (Amount <= 0)
            {
                Entity.RemoveComponent(this);
            }
        }
    }

    public class Chained : SummerJam1Component//todo
    {
        public IEntity ChainedTo { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
        }

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.DrawnCard == Entity || args.DrawnCard == ChainedTo)
            {
                
            }
        }
    }
}
