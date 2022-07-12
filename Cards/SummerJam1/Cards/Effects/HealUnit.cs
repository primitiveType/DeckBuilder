using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Units;

namespace SummerJam1.Cards.Effects
{
    public class HealUnit : SummerJam1Component, IEffect
    {
        [JsonProperty] public int HealAmount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            Entity.AddComponent<DescriptionComponent>().Description = $"Heal a unit for {HealAmount} health.";
        }

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
    }

    public class GrantArmorToUnit : SummerJam1Component, IEffect
    {
        [JsonProperty] public int BlockAmount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            Entity.AddComponent<DescriptionComponent>().Description = $"Gain {BlockAmount} block.";
        }

        public bool DoEffect(IEntity target)
        {
            Unit unit = target.GetComponentInChildren<Unit>();
            
            if (unit == null)
            {
                return false;
            }
            
            Armor armor = unit.Entity.GetOrAddComponent<Armor>();


            armor.Amount += BlockAmount;
            return true;
        }
    }
}
