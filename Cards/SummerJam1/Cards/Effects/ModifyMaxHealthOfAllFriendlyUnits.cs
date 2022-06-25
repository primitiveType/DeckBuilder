using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class ModifyMaxHealthOfAllFriendlyUnits : ModifyComponentOfAllFriendlyUnits<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} the max health of all friendly units by {Amount}.";
            }
        }

        protected override void ModifyComponent(Health component)
        {
            component.SetMax(component.Max + Amount);
        }
    }
}