using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SummerJam1
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SummerJam1CardAsset
    {
        BlackPepper,
        HerbsAndSpices,
        Wasabi,
        Potatoes,
        Milk,
        SpicyPepper,
        PrepTalk,
        Starter,
        Soup,
        Dice,
        Cheddar,
        Tofu,
        ApronPockets,
        Beef,
        Butter
    }
}