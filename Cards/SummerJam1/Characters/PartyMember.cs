using Newtonsoft.Json;

namespace SummerJam1.Characters
{
    public class PartyMember : SummerJam1Component
    {
        [JsonProperty] public bool IsActive { get; set; } = true;
        [JsonProperty] public string DisplayName { get; set; } = "Party Member";
    }
}
