using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class ModifyStrengthOfAllFriendlyUnits : ModifyComponentOfAllFriendlyUnits<Strength>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"{ReduceIncrease} the strength of all friendly units by {Amount}.";
            }
        }

        protected override void ModifyComponent(Strength component)
        {
            component.Amount += Amount;
        }
    }
}