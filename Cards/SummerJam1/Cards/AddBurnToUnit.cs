using System;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class DiscardRandomCard : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            Game.Battle.Hand.DiscardRandom();
            return true;
        }
    }

    public class GainEnergy : SummerJam1Component, IEffect
    {
        public int Amount { get; set; }

        public bool DoEffect(IEntity target)
        {
            Game.Player.CurrentEnergy += Amount;
            return true;
        }
    }

    public class AddBurnToUnit : SummerJam1Component, IEffect
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Amount} burn to a unit.";
            }
        }

        [JsonProperty] public int Amount { get; set; }

        public bool DoEffect(IEntity target)
        {
            IEntity unit = target.GetComponentInChildren<Unit>()?.Entity;
            if (unit == null)
            {
                return false;
            }

            Burn burn = unit.GetOrAddComponent<Burn>();
            burn.Amount += Amount;
            return true;
        }
    }

    public class AddBurnToAllUnits : ModifyComponentOfAllEnemyUnits<Burn>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Math.Abs(Amount)} Burn to all enemy units.";
            }
        }

        protected override void ModifyComponent(Burn component)
        {
            component.Amount += Amount;
        }
    }
}
