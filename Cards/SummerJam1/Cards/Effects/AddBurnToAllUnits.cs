using System;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Cards
{
    public class AddBurnToAllUnits : ModifyComponentOfAllEnemyUnits<Burn>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Add {Math.Abs(Amount)} Burn to all enemy units.";
            }
        }

        protected override void ModifyComponent(Burn component)
        {
            component.Amount += Amount;
        }
    }
}