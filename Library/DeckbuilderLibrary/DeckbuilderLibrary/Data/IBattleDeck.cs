using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    public interface IBattleDeck : IGameEntity
    {
        IPile DrawPile { get; }
        IPile HandPile { get; }
        IPile DiscardPile { get; }
        IPile ExhaustPile { get; }

        IPile DiscoverPile { get; }

        IEnumerable<Card> AllCards();
        void TrySendToPile(Card card, PileType pileType);
        IList<Card> GetPileCards(PileType pileType);
    }
}