using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class HealPlayer : SummerJam1Component, IEffect
    {
        [JsonProperty] public int HealAmount { get; private set; }

        public bool DoEffect(IEntity target)
        {
            Game.Player.Entity.GetComponent<Health>().TryHeal(HealAmount, Entity);
            return true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.AddComponent<DescriptionComponent>().Description = $"Heal for {HealAmount}.";
            }
        }
    }
}