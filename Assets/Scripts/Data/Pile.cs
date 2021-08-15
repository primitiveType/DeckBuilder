using System.Collections;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json;

/// <summary>
/// Not sure if this class is necessary. It's just a list that will fire events, essentially. Events aren't hooked up yet though.
/// Also it probably makes more sense to listen to the deck object so that you always see where the card came from and is going.
/// </summary>
public class Pile : GameEntity
{
    [JsonProperty] public PileType PileType { get; private set; }

    

    [JsonConstructor]
    public Pile(int id, PileType pileType, Properties properties, List<Card> cards) : base(id, properties)
    {
        PileType = pileType;
        Cards = cards;
    }


    public Pile(PileType pileType, IContext context) : base(context)
    {
        PileType = pileType;
    }

    [JsonProperty] public List<Card> Cards { get; private set; } = new List<Card>();

}