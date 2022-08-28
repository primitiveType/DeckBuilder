using Api;
using CardsAndPiles;
using SummerJam1.Cards;

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

    public class Frozen : SummerJam1Component
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.Blockers.Add(CardBlockers.FROZEN);
            }
        }

     
    }
}
