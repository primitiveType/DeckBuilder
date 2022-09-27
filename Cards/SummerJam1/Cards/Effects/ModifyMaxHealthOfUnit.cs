using System;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class ModifyMaxHealthOfUnit : ModifyComponentOfUnit<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} a unit's max health by {Math.Abs(Amount)}.";
            }
        }

        protected override void ModifyComponent(Health component)
        {
            component.SetMax(component.Max + Amount);
        }
    }
}