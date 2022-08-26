using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class TransformAfterAttacked : SummerJam1Component
    {
        [JsonProperty] public int TurnsRemaining { get; set; } = 1;

        [OnRequestDamageMultipliers]
        private void OnRequestDealDamage(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Target != Entity)
            {
                return;
            }

            Unit unit = args.Source.GetComponent<Unit>();

            args.Multiplier.Add(0); //prevent all damage.

            if (unit == null || args.Source.GetComponent<Food>() == null)
            {
                return;
            }


            Entity.GetOrAddComponent<ChangeVisualOnTransform>().UnitAsset =
                unit.Entity.GetComponent<VisualComponent>().AssetName;

            Entity.GetOrAddComponent<GainStrengthOnTransform>().StrengthToAdd =
                unit.Entity.GetComponent<Strength>().Amount;

            Entity.GetOrAddComponent<GainHealthOnTransform>().HealthToAdd =
                unit.Entity.GetComponent<Health>().Max - 1;

            Entity.RemoveComponent(this);
            Events.OnUnitTransformed(new UnitTransformedEventArgs(Entity));
        }
    }
}