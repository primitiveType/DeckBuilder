using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class Deck : GameEntity
{
    public readonly Pile DrawPile = new Pile(CardPile.DrawPile);
    public readonly Pile HandPile = new Pile(CardPile.HandPile);
    public readonly Pile DiscardPile = new Pile(CardPile.DiscardPile);
    public readonly Pile ExhaustPile = new Pile(CardPile.ExhaustPile);

    public IEnumerable<Card> AllCards()
    {
        foreach (Card card in DrawPile)
        {
            yield return card;
        }

        foreach (Card card in HandPile)
        {
            yield return card;
        }

        foreach (Card card in DiscardPile)
        {
            yield return card;
        }

        foreach (Card card in ExhaustPile)
        {
            yield return card;
        }
    }


    public void SendToPile(Card card, CardPile pile)
    {
        CardPile previousPile;
        if (DrawPile.Remove(card))
        {
            previousPile = CardPile.DrawPile;
        }
        else if (HandPile.Remove(card))
        {
            previousPile = CardPile.HandPile;
        }
        else if (ExhaustPile.Remove(card))
        {
            previousPile = CardPile.ExhaustPile;
        }
        else if (DiscardPile.Remove(card))
        {
            previousPile = CardPile.DiscardPile;
        }
        else
        {
            //There might actually be cases where this is legal, we'll see.
            throw new ArgumentException($"Tried to send card to {pile} that does not exist in deck!");
        }

        if (pile == previousPile)
        {
            Debug.LogWarning($"Card with id {card.Id} sent to {pile} when it was already there!");
        }


        GetPileCards(pile).Add(card);
        GameEvents.InvokeCardMoved(this, new CardMovedEventArgs(card.Id, pile, previousPile));
    }

    private IList<Card> GetPileCards(CardPile pile)
    {
        switch (pile)
        {
            case CardPile.DrawPile:
                return DrawPile;
            case CardPile.HandPile:
                return HandPile;
            case CardPile.DiscardPile:
                return DiscardPile;
            case CardPile.ExhaustPile:
                return ExhaustPile;
            default:
                throw new ArgumentOutOfRangeException(nameof(pile), pile, null);
        }
    }
}

public delegate void CardMovedEvent(object sender, CardMovedEventArgs args);

public struct CardMovedEventArgs
{
    public CardPile PreviousPile;
    public CardPile NewPile;
    public int MovedCard;

    public CardMovedEventArgs(int movedCard, CardPile newPile, CardPile previousPile)
    {
        MovedCard = movedCard;
        NewPile = newPile;
        PreviousPile = previousPile;
    }
}

public enum CardPile
{
    DrawPile,
    HandPile,
    DiscardPile,
    ExhaustPile
}