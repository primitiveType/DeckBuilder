using System;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public class Deck : GameEntity
{
    public Pile DrawPile { get; }
    public Pile HandPile { get; }
    public Pile DiscardPile { get; }
    public Pile ExhaustPile { get; }

    [JsonConstructor]
    public Deck( int id, Properties properties, Pile drawPile, Pile handPile, Pile discardPile, Pile exhaustPile) : base(id, properties)
    {
        DrawPile = drawPile;
        HandPile = handPile;
        DiscardPile = discardPile;
        ExhaustPile = exhaustPile;
    }

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
            Debug.LogWarning($"Card with id {card.Id} sent to {pileType} when it was already there!");
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

    public Deck(IContext context) : base(context)
    {
        DrawPile = new Pile(PileType.DrawPile, Context);
        HandPile = new Pile(PileType.HandPile, Context);
        DiscardPile = new Pile(PileType.DiscardPile, Context);
        ExhaustPile = new Pile(PileType.ExhaustPile, Context);
    }
}