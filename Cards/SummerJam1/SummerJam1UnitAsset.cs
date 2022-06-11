using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SummerJam1
{
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