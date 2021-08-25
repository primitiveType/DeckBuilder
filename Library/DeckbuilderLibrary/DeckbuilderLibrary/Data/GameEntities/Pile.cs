using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    /// <summary>
    /// Not sure if this class is necessary. It's just a list that will fire events, essentially. Events aren't hooked up yet though.
    /// Also it probably makes more sense to listen to the deck object so that you always see where the card came from and is going.
    /// </summary>
    internal class Pile : GameEntity, IPile
    {
    
        [JsonProperty] public List<Card> Cards { get; private set; } = new List<Card>();

        [JsonProperty] public PileType PileType { get;  set; }
    }
}