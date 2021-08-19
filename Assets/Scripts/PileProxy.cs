using UnityEngine;
using System.Linq;
using Data;
using System.Collections.Generic;

public class PileProxy<TCardProxy> : Proxy<Pile> where TCardProxy : CardProxy  
{//I think this thing would probably only care about how many cards are in it... and might have inheritors for different
    //types of piles. Should it also initialize card proxies?


    [SerializeField] TCardProxy CardProxyPrefab;

    protected Dictionary<int, TCardProxy> CardProxies { get; set;  } = new Dictionary<int, TCardProxy>();

    protected override void OnInitialize()
    {
        foreach(Card card in GameEntity.AsEnumerable())
        {
            TCardProxy cardProxy = Instantiate(CardProxyPrefab);
            cardProxy.Initialize(card);

            CardProxies.Add(card.Id, cardProxy);
        }
        Debug.Log($"Initialized pile proxy with id {GameEntity.Id}.");
  
        Injector.GameEventHandler.CardMoved += GameEventHandlerOnCardMoved;
    }

    protected virtual void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        //not sure pile type needs to exist. maybe that enum can just go away... not sure.
        if (args.NewPile == GameEntity.PileType && args.PreviousPile != GameEntity.PileType)
        {//this pile is receiving a card, so we create a proxy for it. That way different piles can display cards differently.
            CreateCardProxy(args.MovedCard);
        }
        else if (args.NewPile != GameEntity.PileType && args.PreviousPile == GameEntity.PileType)
        {
            //destroy the card that's being moved away. the other pile should create a new proxy.
            //Not sure how this will work with timing/animations and such.
            DestroyCardProxy(args.MovedCard);
        }
    }

    protected virtual void DestroyCardProxy(int argsMovedCard)
    {
        TCardProxy CardProxy = CardProxies[argsMovedCard];
        CardProxies.Remove(argsMovedCard);
        Destroy(CardProxy.gameObject);
    }

    protected virtual void CreateCardProxy(int argsMovedCard)
    {
        TCardProxy cardProxy = Instantiate(CardProxyPrefab);
        Card cardForProxy = Injector.GlobalApi.GetCurrentBattle().Deck.AllCards().First(card => card.Id == argsMovedCard);
        cardProxy.Initialize(cardForProxy);
        CardProxies.Add(argsMovedCard, cardProxy);
    }

 
}