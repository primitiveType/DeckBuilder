using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class HealUnit : SummerJam1Component, IEffect
    {
        [JsonProperty] public int HealAmount { get; private set; }

        public bool DoEffect(IEntity target)
        {
            IHealable unit = target.GetComponentInChildren<IHealable>();

            if (unit == null)
            {
                return false;
            }


            unit.TryHeal(HealAmount, Entity);
            return true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Entity.AddComponent<DescriptionComponent>().Description = $"Heal a unit for {HealAmount} health.";
        }
    }
}
