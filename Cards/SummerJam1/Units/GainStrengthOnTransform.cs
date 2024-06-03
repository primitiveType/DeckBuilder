using Newtonsoft.Json;

namespace SummerJam1.Units
{
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
