using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public class DamageUnitCard : SummerJam1Component, IEffect
    {
        [JsonProperty] public int DamageAmount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description =
                $"Deal {DamageAmount} damage to target Unit.";
        }


        public bool DoEffect(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(DamageAmount, Entity);
            return true;
        }

    }
}