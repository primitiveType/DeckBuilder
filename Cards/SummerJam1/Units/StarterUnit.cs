using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit
    {
    }

    public class ChangeVisualOnTransform : SummerJam1Component
    {
        [JsonProperty] public SummerJam1UnitAsset UnitAsset { get; set; }

        [OnUnitTransformed]
        private void UnitTransformed(object sender, UnitTransformedEventArgs args)
        {
            if (args.Entity == Entity)
            {
                var visual = Entity.GetOrAddComponent<VisualComponent>();
                visual.AssetName = UnitAsset;
            }
        }
    }

    public class TransformAfterTurns : SummerJam1Component
    {
        [JsonProperty] public int TurnsRemaining { get; set; } = 1;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            TurnsRemaining--;

            if (TurnsRemaining <= 0)
            {
                Entity.RemoveComponent(this);
            }

            Events.OnUnitTransformed(new UnitTransformedEventArgs(Entity));
        }
    }


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
    
    public class GainStrengthOnTransform : SummerJam1Component
    {
        [JsonProperty] public int StrengthToAdd { get; set; }

        [OnUnitTransformed]
        private void UnitTransformed(object sender, UnitTransformedEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Strength strength = Entity.GetOrAddComponent<Strength>();
                strength.Amount = strength.Amount += StrengthToAdd;
            }
        }
    }
}