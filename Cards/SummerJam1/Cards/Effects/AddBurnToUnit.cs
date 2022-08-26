using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class AddBurnToUnit : SummerJam1Component, IEffect
    {
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

        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Amount} burn to a unit.";
            }
        }
    }
}