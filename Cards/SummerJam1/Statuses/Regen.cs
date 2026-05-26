using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class Regen : SummerJam1Component, IAmount, IStatusEffect, ITooltip
    {
        public int Amount { get; set; }
        public string Tooltip => $"Regen - At start of turn, heals {Amount}.";

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
}
