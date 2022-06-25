using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class ModifyHealthOfAllFriendlyUnits : ModifyComponentOfAllFriendlyUnits<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = $"Heal all friendly units for {Amount}.";
            }
        }

        protected override void ModifyComponent(Health component)
        {
            component.TryHeal(Amount, Entity);
        }
    }
}