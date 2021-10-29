using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreationPileProxy : PileProxy<DeckCreationCardProxy>
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        GameEntity.Context.Events.DiscoverCards += OnCardsDiscovered;
    }

    private int NumCards;
    private int CardsPerRow = 4;

    [SerializeField]
    private float CardWidth;

    [SerializeField]
    private float ScreenWidth;

    [SerializeField]
    private float CardHeight;

    [SerializeField]
    private float CardSeperation;

    [SerializeField]
    private Vector3 TopLeftPosition;

    [SerializeField]
    private Transform CardParent;

    private void OnCardsDiscovered(object sender, DiscoverCardsEventArgs args)
    {
        NumCards = args.NumOptions;

    }


    protected override DeckCreationCardProxy CreateCardProxy(int argsMovedCard)
    {
        var proxy = base.CreateCardProxy(argsMovedCard);

        proxy.transform.SetParent(CardParent);

        CardProxies[argsMovedCard].DisplayIndex = CardProxies.Count - 1;

        PlaceCardProxy(proxy);

        return proxy;
    }

    private void PlaceCardProxy(DeckCreationCardProxy cardProxy)
    {
        int row = cardProxy.DisplayIndex / CardsPerRow;
        int column = cardProxy.DisplayIndex % CardsPerRow;

        float xOffset = column * (CardWidth * 2 + CardSeperation);
        float yOffset = row * (CardHeight * 2 + CardSeperation);

        cardProxy.transform.localPosition = TopLeftPosition + new Vector3(xOffset, -yOffset, 0);      

    }

    protected override void DestroyCardProxy(int argsMovedCard)
    {
       
    }

    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);
    }


}
