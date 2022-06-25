using Api;
using CardsAndPiles.Components;
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
            Unit unit = target.GetComponentInChildren<Unit>();

            if (unit == null)
            {
                return false;
            }


            unit.Entity.GetOrAddComponent<MultiAttack>().Amount++;
            return true;
        }
    }
}