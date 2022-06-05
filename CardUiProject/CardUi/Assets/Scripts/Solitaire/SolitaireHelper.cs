using System;
using System.Collections.Generic;
using Api;
using CardsAndPiles;
using Common;
using Solitaire;
using UnityEngine;
using UnityEngine.Serialization;
using DeckPile = Solitaire.DeckPile;


public class SolitaireHelper : MonoBehaviourSingleton<SolitaireHelper>
{
    public List<Sprite> Sprites;
    public GameObject CardPrefab => m_CardPrefab;


    [SerializeField] private GameObject m_CardPrefab;

    [FormerlySerializedAs("DeckPile")] [SerializeField]
    private PileView m_DeckPileView;

    [SerializeField] private PileView m_HandPileView;

    [SerializeField] private PileView m_BankPrefab;


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

    protected override void Awake()
    {
        base.Awake();
        SetupGame();
    }

    private void SetupGame()
    {
        Context context = new Context(new CardEvents());
        IEntity root = context.CreateEntity();
        IEntity game = context.CreateEntity();
        game.AddComponent<StandardDeckCardViewBridge>();
        game.TrySetParent(root);
        Game = game.AddComponent<SolitaireGame>();
        PileViewBridge deckBridge = game.GetComponentInChildren<DeckPile>().Entity.AddComponent<PileViewBridge>();
        deckBridge.gameObject = m_DeckPileView.gameObject;
        m_DeckPileView.SetModel(deckBridge.Entity);
        PileViewBridge handBridge = game.GetComponentInChildren<HandPile>().Entity.AddComponent<PileViewBridge>();
        handBridge.gameObject = m_HandPileView.gameObject;
        m_HandPileView.SetModel(handBridge.Entity);

        List<BankPile> bankPiles = game.GetComponentsInChildren<BankPile>();

        int i = 0;
        foreach (var bankPile in bankPiles)
        {
            i++;
            PileViewBridge bankBridge = bankPile.Entity.AddComponent<PileViewBridge>();
            var bankView = Instantiate(m_BankPrefab);
            bankView.transform.position = transform.position + new Vector3(i * 5, 0, 0);
            bankBridge.gameObject = bankView.gameObject;
            bankView.SetModel(bankBridge.Entity);
        }
        
        Game.StartGame();
    }
}