using Api;
using Newtonsoft.Json;
using SummerJam1.Characters;

namespace SummerJam1.Cards
{
    public class CardOwner : SummerJam1Component
    {
        [JsonProperty] public int OwnerId { get; set; } = -1;

        [JsonIgnore]
        public IEntity Owner => Game.Party?.GetMemberById(OwnerId);
    }
}
