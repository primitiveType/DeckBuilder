using System;

namespace DeckbuilderLibrary.Data
{
    [Serializable]
    public enum PileType
    {
        None,
        DrawPile,
        HandPile,
        DiscardPile,
        ExhaustPile,
        DiscoverPile
    }
}