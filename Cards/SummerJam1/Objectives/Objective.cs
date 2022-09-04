using Newtonsoft.Json;

namespace SummerJam1.Objectives
{
    public abstract class Objective : SummerJam1Component
    {
        [JsonProperty] public bool Completed { get; set; }
        [JsonProperty] public bool Failed { get; set; }
    }
}
