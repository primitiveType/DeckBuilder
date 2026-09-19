using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using PropertyChanged;
using SummerJam1.Cards;

namespace SummerJam1.Cards.Effects
{
    public class DamageUnitCard : SummerJam1Component, IEffect, IDescription, ITooltip
    {
        TargetingType IEffect.Targeting { get; } = TargetingType.Unit;
        [JsonProperty] public int DamageAmount { get; private set; }
        protected virtual int FinalDamage => DamageAmount + Strength;
        [JsonProperty] public int Attacks { get; set; } = 1;
        [JsonProperty] public bool Pierce { get; set; }

        protected int Strength => (Entity.GetComponent<CardOwner>()?.Owner ?? Game.Player.Entity)
            .GetOrAddComponent<Strength>()
            .Amount;

        protected Targeting Targeting => Entity?.GetComponent<Targeting>();

        [DependsOn(nameof(Strength), nameof(DamageAmount), nameof(Attacks))]
        public virtual string Description
        {
            get
            {
                string pierceString = Pierce ? "Pierce." : "";
                if (Attacks == 1)
                {
                    if (Targeting is { Aoe: true })
                    {
                        return $"Deal {FinalDamage} damage to ALL enemies. {pierceString}";
                    }

                    return $"Deal {FinalDamage} damage. {pierceString}";
                }

                if (Targeting is { Aoe: true })
                {
                    return $"Deal {FinalDamage} damage to target and adjacent, {Attacks} times. {pierceString}";
                }

                return $"Deal {FinalDamage} damage, {Attacks} times. {pierceString}";
            }
        }

        public virtual bool DoEffect(IEntity target)
        {
            List<ITakesDamage> units;
            if (Targeting is { Aoe: true })
            {
                units = Game.Battle.MonsterSlots.Entity.GetComponentsInChildren<ITakesDamage>();
            }
            else
            {
                units = new List<ITakesDamage>
                {
                    target.GetComponent<ITakesDamage>()
                };
            }

            if (!units.Any())
            {
                return false;
            }

            for (int i = 0; i < Attacks; i++)
            {
                foreach (var unit in units)
                {
                    unit.TryDealDamage(FinalDamage, Entity);
                }
            }

            return true;
        }

        public string Tooltip => Pierce ? PierceTooltip.PIERCE_TOOLTIP : null;
    }
}
