using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class ModifyMaxHealthOfUnitByStrength : ModifyComponentOfUnit<Health>
    {
        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description = "Increase a unit's max health by its strength.";
            }
        }

        protected override void ModifyComponent(Health component)
        {
            Strength strength = component.Entity.GetComponent<Strength>();
            component.SetMax(component.Max + strength.Amount);
        }
    }
}