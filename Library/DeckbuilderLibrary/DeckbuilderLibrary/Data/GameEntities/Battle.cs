using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    public class Battle : GameEntity
    {
        [JsonProperty] public Actor Player { get; set; }
        [JsonProperty] public List<Actor> Enemies { get; set; }
        [JsonProperty] public Deck Deck { get; set; }

    }
}