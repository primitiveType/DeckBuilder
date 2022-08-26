using Api;
using CardsAndPiles;

namespace SummerJam1.Statuses
{
    public class Regen : EnabledWhenInEncounterSlot, IAmount, IStatusEffect
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
}
