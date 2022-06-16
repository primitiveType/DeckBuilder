using System;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;
using SummerJam1.Units;

namespace SummerJam1
{
    public class AddMultiAttackToUnit : SummerJam1Component, IEffect
    {
        protected override void Initialize()
        {
            base.Initialize();

            Entity.AddComponent<DescriptionComponent>().Description = $"Grant 1 multi-attack to a unit.";
        }

        public bool DoEffect(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            Unit unit = target.GetComponentInChildren<Unit>();

            if (unit == null)
            {
                return false;
            }


            unit.Entity.GetOrAddComponent<MultiAttack>().Amount++;
            return true;
        }
    }

    public abstract class ModifyComponentOfUnit<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            Unit unit = target.GetComponentInChildren<Unit>();

            if (unit == null)
            {
                return false;
            }

            ModifyComponent(unit.Entity.GetOrAddComponent<T>());
            return true;
        }

        protected abstract void ModifyComponent(T component);
    }

    public class ModifyStrengthOfUnit : ModifyComponentOfUnit<Strength>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} a unit's strength by {Math.Abs(Amount)}.";
        }

        protected override void ModifyComponent(Strength component)
        {
            component.Amount += Amount;
        }
    }

    public class ModifyMaxHealthOfUnit : ModifyComponentOfUnit<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description += $"{ReduceIncrease} a unit's max health by {Math.Abs(Amount)}.";
        }

        protected override void ModifyComponent(Health component)
        {
            component.SetMax(component.Max + Amount);
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
    }

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
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

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