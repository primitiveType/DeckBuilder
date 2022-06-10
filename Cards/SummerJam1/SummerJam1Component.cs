using Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SummerJam1
{
    public class SummerJam1Component : Component
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;
    }
    public class VisualComponent : SummerJam1Component
    {
        public SummerJam1UnitAsset AssetName { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SummerJam1UnitAsset
    {
        IceCream,
        HeadCheese,
        Sandwich,
        Noodles,
        Tofu
    }
}