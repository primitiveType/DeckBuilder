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
        StandardDeckCard card = Entity.GetComponent<StandardDeckCard>();
        if (card != null)
        {
            MakeCard(card);
        }


        Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        foreach (IEntity child in Entity.Children)
        {
            AddBridgeIfMissing(child);
        }
    }

    private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (IEntity item in e.NewItems)
            {
                AddBridgeIfMissing(item);
            }
        }
    }

    private static void AddBridgeIfMissing(IEntity item)
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
        card.SetModel(cardModel.Entity);
    }

    public GameObject gameObject { get; private set; }
}