using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards
{
    public class HealLowestUnit : SummerJam1Component, IEffect
    {
        [JsonProperty] public int HealAmount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            Entity.AddComponent<DescriptionComponent>().Description = $"Heal a unit for {HealAmount} health.";
        }

        public bool DoEffect(IEntity target)
        {
            target = Game.Battle.GetFriendlies().OrderBy(entity => entity.GetComponent<Health>().Amount)
                .FirstOrDefault();

            if (target == null)
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
    }
}