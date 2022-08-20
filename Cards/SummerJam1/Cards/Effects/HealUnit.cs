using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class AddStealth : SummerJam1Component, IEffect, IAmount, IDescription
    {
        public int Amount { get; set; }

        public bool DoEffect(IEntity target)
        {
            return Game.Player.Entity.GetOrAddComponent<Stealth>().TryUseStealth(-Amount);
        }

        public string Description => $"Gain {Amount} Stealth.";
    }

    public class HealPlayer : SummerJam1Component, IEffect
    {
        [JsonProperty] public int HealAmount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.AddComponent<DescriptionComponent>().Description = $"Heal for {HealAmount}.";
            }
        }

        public bool DoEffect(IEntity target)
        {
            Game.Player.Entity.GetComponent<Health>().TryHeal(HealAmount, Entity);
            return true;
        }
    }

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

    public class GrantArmorToPlayer : SummerJam1Component, IEffect, IDescription, ITooltip
    {
        [JsonProperty] public int BlockAmount { get; private set; }


        public bool DoEffect(IEntity target)
        {
            Armor armor = Game.Player.Entity.GetOrAddComponent<Armor>();
            armor.Amount += BlockAmount;
            return true;
        }

        public string Description => $"Gain {BlockAmount} block.";
        public string Tooltip => Tooltips.ARMOR_TOOLTIP;
    }
}
