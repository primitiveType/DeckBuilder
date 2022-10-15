using Api;
using Newtonsoft.Json;

namespace SummerJam1.Units.Effects
{
    public class MultiAttack : SummerJam1Component, IAmount
    {
        [JsonProperty] public int Amount { get; set; }
    }
}