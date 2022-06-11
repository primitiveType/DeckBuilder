using Newtonsoft.Json;

namespace SummerJam1
{
    public class Strength : SummerJam1Component
    {
        [JsonProperty] public int Amount { get; set; }
    }
}