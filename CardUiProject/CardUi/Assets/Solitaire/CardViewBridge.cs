using System.Collections.Specialized;
using Api;
using Solitaire;
using UnityEngine;
using Component = Api.Component;
using Common;

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