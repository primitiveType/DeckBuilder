using System;

namespace DeckbuilderLibrary.Data
{
    [Serializable]
    public enum PileType
    {
        DrawPile,
        HandPile,
        DiscardPile,
        ExhaustPile,
        DiscoverPile,
        None
    }
}