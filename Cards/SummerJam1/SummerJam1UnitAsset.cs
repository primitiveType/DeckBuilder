using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SummerJam1
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SummerJam1UnitAsset
    {
        Player,
        IceCream,
        HeadCheese,
        Sandwich,
        Noodles,
        Tofu
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SummerJam1CardAsset
    {
        BlackPepper,
        SpicyPepper,
        PrepTalk
    }
}