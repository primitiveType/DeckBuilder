using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    internal class Deck : GameEntity, IDeck
    {
        public IPile DrawPile { get; private set; }
        public IPile HandPile { get; private set; }
        public IPile DiscardPile { get; private set; }
        public IPile ExhaustPile { get; private set; }


        public IEnumerable<Card> AllCards()
        {
            foreach (Card card in DrawPile.Cards)
            {
                yield return card;
            }

            foreach (Card card in HandPile.Cards)
            {
                yield return card;
            }

            foreach (Card card in DiscardPile.Cards)
            {
                yield return card;
            }

            foreach (Card card in ExhaustPile.Cards)
            {
                yield return card;
            }
        }


        public void TrySendToPile(Card card, PileType pileType)
        {
            PileType previousPileType;
            if (DrawPile.Cards.Remove(card))
            {
                previousPileType = PileType.DrawPile;
            }
            else if (HandPile.Cards.Remove(card))
            {
                previousPileType = PileType.HandPile;
            }
            else if (ExhaustPile.Cards.Remove(card))
            {
                previousPileType = PileType.ExhaustPile;
            }
            else if (DiscardPile.Cards.Remove(card))
            {
                previousPileType = PileType.DiscardPile;
            }
            else
            {
                //There might actually be cases where this is legal, we'll see.
                throw new ArgumentException($"Tried to send card to {pileType} that does not exist in deck!");
            }

            if (pileType == previousPileType)
            {
                Console.WriteLine($"Card with id {card.Id} sent to {pileType} when it was already there!");
            }


            GetPileCards(pileType).Add(card);
            ((IInternalGameEventHandler)Context.Events).InvokeCardMoved(this,
                new CardMovedEventArgs(card.Id, pileType, previousPileType));
        }

        public IList<Card> GetPileCards(PileType pileType)
        {
            switch (pileType)
            {
                case PileType.DrawPile:
                    return DrawPile.Cards;
                case PileType.HandPile:
                    return HandPile.Cards;
                case PileType.DiscardPile:
                    return DiscardPile.Cards;
                case PileType.ExhaustPile:
                    return ExhaustPile.Cards;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pileType), pileType, null);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (DrawPile == null)
            {
                DrawPile = Context.CreateEntity<Pile>();
                ((Pile)DrawPile).PileType = PileType.DrawPile;
            }

            if (HandPile == null)
            {
                HandPile = Context.CreateEntity<Pile>();
                ((Pile)HandPile).PileType = PileType.HandPile;
            }

            if (DiscardPile == null)
            {
                DiscardPile = Context.CreateEntity<Pile>();
                ((Pile)DiscardPile).PileType = PileType.DiscardPile;
            }

            if (ExhaustPile == null)
            {
                ExhaustPile = Context.CreateEntity<Pile>();
                ((Pile)ExhaustPile).PileType = PileType.ExhaustPile;
            }
        }
    }
}