using System;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class ModifyHealthOfFrontFriendlyUnit : ModifyComponentOfFrontFriendlyUnit<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Heal the front unit by {Math.Abs(Amount)}.";
            }
        }

        protected override void ModifyComponent(Health component)
        {
            component.TryHeal(Amount, Entity);
        }
    }
}