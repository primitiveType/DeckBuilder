using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class Regen : SummerJam1Component, IAmount, IStatusEffect
    {
        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetComponent<Health>().TryHeal(Amount, Entity);
            Amount--;
            if (Amount <= 0)
            {
                Entity.RemoveComponent(this);
            }
        }
    }

    public interface IStatusEffect
    {
    }

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
