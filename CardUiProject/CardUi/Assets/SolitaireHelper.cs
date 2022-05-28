using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireHelper : MonoBehaviourSingleton<SolitaireHelper>
{
    public List<Sprite> Sprites;
    public bool GameStarted { get; private set; }

    [SerializeField] private StandardDeckCard CardPrefab;

    [SerializeField] private Pile DeckPile;

    public Sprite GetCardSprite(int number, Suit suit)
    {
        //hearts, spades, clubs, diamonds 13 each
        int startIndex;
        switch (suit)
        {
            case Suit.Hearts:
                startIndex = 0;
                break;
            case Suit.Spades:
                startIndex = 13;
                break;
            case Suit.Clubs:
                startIndex = 26;
                break;
            case Suit.Diamonds:
                startIndex = 39;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(suit), suit, null);
        }

        startIndex += 4;//card backs and jokers.
        return Sprites[startIndex + number];
    }

    private void Awake()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        GameStarted = false;
        for (int i = 0; i < 13; i++)
        {
            MakeCard(i, Suit.Clubs);
            MakeCard(i, Suit.Hearts);
            MakeCard(i, Suit.Spades);
            MakeCard(i, Suit.Diamonds);
        }

        GameStarted = true;
    }

    private void MakeCard(int i, Suit suit)
    {
        StandardDeckCard card = Instantiate(CardPrefab).GetComponent<StandardDeckCard>();
        card.SetCard(i, suit);
        DeckPile.ReceiveItem(card.GetComponentInParent<IPileItem>());
    }
}