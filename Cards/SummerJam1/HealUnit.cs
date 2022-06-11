using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class HealUnit : SummerJam1Card
    {
        [JsonProperty] public int HealAmount { get; private set; }

        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            IHealable unit = target.GetComponentInChildren<IHealable>();

            if (unit == null)
            {
                return false;
            }


            unit.TryHeal(HealAmount, Entity);
            return true;
        }

        public override string Description => $"Heal a unit for {HealAmount} health.";
    }
}