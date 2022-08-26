using Api;
using CardsAndPiles.Components;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public class DoubleMaxHealthOfUnit : SummerJam1Component, IEffect
    {
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

        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = "Double the Max Health of a Unit.";
            }
        }
    }
}