using Api;
using CardsAndPiles;

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
}