using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class ChangeVisualOnTransform : SummerJam1Component
    {
        [JsonProperty] public string UnitAsset { get; set; }

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
}
