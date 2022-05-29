using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Api;
using Solitaire;
using UnityEngine;
using UnityEngine.Serialization;
using Component = Api.Component;

public class SolitaireHelper : MonoBehaviourSingleton<SolitaireHelper>
{
    public List<Sprite> Sprites;
    public StandardDeckCardView CardPrefab => m_CardPrefab;


    [SerializeField] private StandardDeckCardView m_CardPrefab;

    [FormerlySerializedAs("DeckPile")] [SerializeField]
    private PileView m_DeckPileView;

    [SerializeField] private PileView m_HandPileView;

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

        startIndex += 4; //card backs and jokers.
        return Sprites[startIndex + number];
    }

    private void Awake()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        var root = new Entity();
        var game = new Entity();
        game.AddComponent<CardViewBridge>();
        game.SetParent(root);
        Game = game.AddComponent<SolitaireGame>();
        PileViewBridge deckBridge = game.GetComponentInChildren<DeckPile>().Parent.AddComponent<PileViewBridge>();
        deckBridge.gameObject = m_DeckPileView.gameObject;
        m_DeckPileView.SetModel(deckBridge.Parent);
        PileViewBridge handBridge = game.GetComponentInChildren<HandPile>().Parent.AddComponent<PileViewBridge>();
        handBridge.gameObject = m_HandPileView.gameObject;
        m_HandPileView.SetModel(handBridge.Parent);
    }
}

public class ViewFactory
{
}

public class PileViewBridge : Component, IGameObject
{ //piles are pre-determined so there is much less logic.
    public GameObject gameObject { get; set; }
}


public class CardViewBridge : Component, IGameObject
{
    public StandardDeckCardView CardPrefab => SolitaireHelper.Instance.CardPrefab;

    protected override void Initialize()
    {
        base.Initialize();
        StandardDeckCard card = Parent.GetComponent<StandardDeckCard>();
        if (card != null)
        {
            MakeCard(card);
        }


        Parent.Children.CollectionChanged += ChildrenOnCollectionChanged;
        foreach (Entity child in Parent.Children)
        {
            AddBridgeIfMissing(child);
        }
    }

    private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (Entity item in e.NewItems)
            {
                AddBridgeIfMissing(item);
            }
        }
    }

    private static void AddBridgeIfMissing(Entity item)
    {
        if (item.GetComponent<CardViewBridge>() == null)
        {
            item.AddComponent<CardViewBridge>();
        }
    }

    private void MakeCard(StandardDeckCard cardModel)
    {
        StandardDeckCardView card = GameObject.Instantiate(CardPrefab).GetComponent<StandardDeckCardView>();
        gameObject = card.gameObject;
        card.SetModel(cardModel.Parent);
    }

    public GameObject gameObject { get; private set; }
}