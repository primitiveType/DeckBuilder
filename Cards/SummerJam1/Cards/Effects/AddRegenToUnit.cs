using System;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards.Effects
{
    public class AddRegenToUnit : SummerJam1Component, IEffect
    {
        private bool _initialized;
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
            if (_initialized)
            {
                throw new Exception("WHAT");
            }

            _initialized = true;
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Amount} regen to a unit.";
            }
        }
    }
}
