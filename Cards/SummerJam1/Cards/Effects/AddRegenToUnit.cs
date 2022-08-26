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
        private bool initialized;
        [JsonProperty] public int Amount { get; set; }


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

        protected override void Initialize()
        {
            base.Initialize();
            if (initialized)
            {
                throw new Exception("WHAT");
            }

            initialized = true;
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Amount} regen to a unit.";
            }
        }
    }
}