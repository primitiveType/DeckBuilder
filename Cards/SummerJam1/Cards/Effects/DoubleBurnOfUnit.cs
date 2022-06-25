using Api;
using CardsAndPiles.Components;
using SummerJam1.Statuses;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class DoubleBurnOfUnit : SummerJam1Component, IEffect
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Double the amount of Burn on a unit.";
            }
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
}