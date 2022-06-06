using Api;

namespace SummerJam1.Units
{
    public class StarterUnitCard : UnitCard
    {
        protected override Unit CreateUnit()
        {
            IEntity unitEntity = Context.CreateEntity(null, entity =>
            {
                entity.AddComponent<StarterUnit>();
            });

            return unitEntity.GetComponent<StarterUnit>();
        }
    }
}