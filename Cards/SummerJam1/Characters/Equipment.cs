using System.Collections.Generic;
using Newtonsoft.Json;

namespace SummerJam1.Characters
{
    public class Equipment : SummerJam1Component
    {
        [JsonProperty] public EquipmentSlot Slot { get; set; }
        [JsonProperty] public List<string> CardPrefabs { get; private set; } = new();
    }
}
