using System;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class ModifyStrengthOfUnit : ModifyComponentOfUnit<Strength>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} a unit's strength by {Math.Abs(Amount)}.";
            }
        }

        protected override void ModifyComponent(Strength component)
        {
            component.Amount += Amount;
        }
    }
}