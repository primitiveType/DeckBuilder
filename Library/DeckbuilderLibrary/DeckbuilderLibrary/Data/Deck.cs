using System;
using System.Collections.Generic;
using Data;
using DeckbuilderLibrary.Data.GameEntities;

public class Deck : GameEntity
{
    public Pile DrawPile { get; private set; }
    public Pile HandPile { get; private set; }
    public Pile DiscardPile { get; private set; }
    public Pile ExhaustPile { get; private set; }


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


    public void SendToPile(Card card, PileType pileType)
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
        Context.Events.InvokeCardMoved(this, new CardMovedEventArgs(card.Id, pileType, previousPileType));
    }

    private IList<Card> GetPileCards(PileType pileType)
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

    public override void Initialize()
    {
        base.Initialize();
        if (DrawPile == null)
        {
            DrawPile = Context.CreateEntity<Pile>();
            DrawPile.PileType = PileType.DrawPile;
        }

        if (HandPile == null)
        {
            HandPile = Context.CreateEntity<Pile>();
            HandPile.PileType = PileType.HandPile;
        }

        if (DiscardPile == null)
        {
            DiscardPile = Context.CreateEntity<Pile>();
            DiscardPile.PileType = PileType.DiscardPile;
        }

        if (ExhaustPile == null)
        {
            ExhaustPile = Context.CreateEntity<Pile>();
            ExhaustPile.PileType = PileType.ExhaustPile;
        }
    }
}