using System;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class AddRegenToUnit : SummerJam1Component, IEffect
    {
        [JsonProperty] public int Amount { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description += $"Add {Amount} regen to a unit.";
        }


        public bool DoEffect(IEntity target)
        {
            IEntity unit = target.GetComponentInChildren<Unit>()?.Entity;
            if (unit == null)
            {
                return false;
            }

            Regen regen = unit.GetOrAddComponent<Regen>();
            regen.Amount += Amount;
            return true;
        }
    }

    public class AddRegenToAllFriendlyUnits : SummerJam1Component, IEffect
    {
        [JsonProperty] public int Amount { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Amount} regen to all friendly units.";
        }

        public bool DoEffect(IEntity target)
        {
            var friendlies = Game.Battle.GetFriendlies();
            foreach (IEntity friendly in friendlies)
            {
                Regen regen = friendly.GetOrAddComponent<Regen>();
                regen.Amount += Amount;
            }

            return true;
        }
    }
}
