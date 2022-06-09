using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Units
{
    public class StarterUnitCard : UnitCard, IDescription
    {
        protected override Unit CreateUnit()
        {
            IEntity unitEntity = Context.CreateEntity(null, entity =>
            {
                entity.AddComponent<StarterUnit>();
            });

            return unitEntity.GetComponent<StarterUnit>();
        }

        public string Description => "Summon a Random Unit.";
    }
}