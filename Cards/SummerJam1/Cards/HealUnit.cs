using System;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards
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

    public class ModifyStrengthOfAllFriendlyUnits : ModifyComponentOfAllFriendlyUnits<Strength>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} the strength of all friendly units by {Amount}.";
        }

        protected override void ModifyComponent(Strength component)
        {
            component.Amount += Amount;
        }
    }

    public class ModifyHealthOfAllFriendlyUnits : ModifyComponentOfAllFriendlyUnits<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Heal all friendly units for {Amount}.";
        }

        protected override void ModifyComponent(Health component)
        {
            component.TryHeal(Amount, Entity);
        }
    }

    public class ModifyMaxHealthOfAllFriendlyUnits : ModifyComponentOfAllFriendlyUnits<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} the max health of all friendly units by {Amount}.";
        }

        protected override void ModifyComponent(Health component)
        {
            component.SetMax(component.Max + Amount);
        }
    }

    public abstract class ModifyComponentOfAllEnemyUnits<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            foreach (IEntity friendly in Game.Battle.GetEnemies())
            {
                ModifyComponent(friendly.GetOrAddComponent<T>());
            }

            return true;
        }

        protected abstract void ModifyComponent(T component);
    }

    public abstract class ModifyComponentOfAllFriendlyUnits<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            foreach (IEntity friendly in Game.Battle.GetFriendlies())
            {
                ModifyComponent(friendly.GetOrAddComponent<T>());
            }

            return true;
        }

        protected abstract void ModifyComponent(T component);
    }

    public class ModifyHealthOfFrontFriendlyUnit : ModifyComponentOfFrontFriendlyUnit<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Heal the front unit by {Math.Abs(Amount)}.";

        }

        protected override void ModifyComponent(Health component)
        {
            component.TryHeal(Amount, Entity);
        }
    }

    public abstract class ModifyComponentOfFrontFriendlyUnit<T> : SummerJam1Component, IEffect where T : Component, new()
    {
        [JsonProperty] public int Amount { get; private set; }
        protected string ReduceIncrease => Amount > 0 ? "Increase" : "Reduce";


        public bool DoEffect(IEntity target)
        {
            IEntity front = Game.Battle.GetFrontMostFriendly();
            if (front == null)
            {
                return false;
            }

            ModifyComponent(front.GetOrAddComponent<T>());


            return true;
        }

        protected abstract void ModifyComponent(T component);
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

    public class ModifyMaxHealthOfUnitByStrength : ModifyComponentOfUnit<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description += $"Increase a unit's max health by its strength.";
        }

        protected override void ModifyComponent(Health component)
        {
            var strength = component.Entity.GetComponent<Strength>();
            component.SetMax(component.Max + strength.Amount);
        }
    }

    public class DoubleMaxHealthOfUnit : SummerJam1Component, IEffect
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Double the Max Health of a Unit.";
        }

        public bool DoEffect(IEntity target)
        {
            Unit unit = target.GetComponentInChildren<Unit>();
            Health health = unit?.Entity.GetComponent<Health>();
            if (health == null)
            {
                return false;
            }

            health.SetMax(health.Max * 2);

            return true;
        }
    }
    
    public class DoubleBurnOfUnit : SummerJam1Component, IEffect
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Double the amount of Burn on a unit.";
        }

        public bool DoEffect(IEntity target)
        {
            Unit unit = target.GetComponentInChildren<Unit>();
            Burn burn = unit?.Entity.GetComponent<Burn>();
            if (burn == null)
            {
                return false;
            }

            burn.Amount *= 2;

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
