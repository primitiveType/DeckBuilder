using Api;
using Newtonsoft.Json;

namespace SummerJam1
{
    public class Strength : SummerJam1Component, IAmount
    {
        [JsonProperty] public int Amount { get; set; }
    }


    public class MultiAttack : SummerJam1Component, IAmount
    {
        [JsonProperty] public int Amount { get; set; }
    }
}
