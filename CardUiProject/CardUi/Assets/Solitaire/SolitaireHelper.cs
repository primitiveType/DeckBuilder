using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Api;
using Common;
using Solitaire;
using UnityEngine;
using UnityEngine.Serialization;

public class SolitaireHelper : MonoBehaviourSingleton<SolitaireHelper>
{
    public List<Sprite> Sprites;
    public StandardDeckCardView CardPrefab => m_CardPrefab;


    [SerializeField] private StandardDeckCardView m_CardPrefab;

    [FormerlySerializedAs("DeckPile")] [SerializeField]
    private PileView m_DeckPileView;

    [SerializeField] private PileView m_HandPileView;
    
    [SerializeField] private List<PileView> m_BankPiles;


    private SolitaireGame Game { get; set; }

    public Sprite GetCardSprite(int number, Suit suit)
    {
        //hearts, spades, clubs, diamonds 13 each
        int startIndex;
        switch (suit)
        {
            case Suit.Hearts:
                startIndex = 0;
                break;
            case Suit.Clubs:
                startIndex = 13;
                break;
            case Suit.Diamonds:
                startIndex = 26;
                break;
            case Suit.Spades:
                startIndex = 39;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(suit), suit, null);
        }

        startIndex += 4; //card backs and jokers.
        return Sprites[startIndex + number];
    }

    private void Awake()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        Entity root = new Entity();
        Entity game = new Entity();
        game.AddComponent<CardViewBridge>();
        game.SetParent(root);
        Game = game.AddComponent<SolitaireGame>();
        PileViewBridge deckBridge = game.GetComponentInChildren<DeckPile>().Parent.AddComponent<PileViewBridge>();
        deckBridge.gameObject = m_DeckPileView.gameObject;
        m_DeckPileView.SetModel(deckBridge.Parent);
        PileViewBridge handBridge = game.GetComponentInChildren<HandPile>().Parent.AddComponent<PileViewBridge>();
        handBridge.gameObject = m_HandPileView.gameObject;
        m_HandPileView.SetModel(handBridge.Parent);

        var bankPiles = game.GetComponentInChildren<BankPile>();
        
        
        var card = game.GetComponentInChildren<HandPile>().Parent.AddComponent<StandardDeckCard>();
        
        card.PropertyChanged += CardOnPropertyChanged;
        card.IsFaceDown = true;

    }

    private void CardOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Debug.Log("Got Property Change!");
    }
}