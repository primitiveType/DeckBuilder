using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class GainHealthOnTransform : SummerJam1Component
    {
        [JsonProperty] public int HealthToAdd { get; set; }

        [OnUnitTransformed]
        private void UnitTransformed(object sender, UnitTransformedEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Health health = Entity.GetOrAddComponent<Health>();
                health.SetMax(health.Max + HealthToAdd);
                health.SetHealth(health.Max);
            }
        }
    }
}