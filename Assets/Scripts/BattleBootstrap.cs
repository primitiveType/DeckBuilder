using System.Collections.Generic;
using Data;
using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] ActorProxy ActorProxyPrefab;
    [SerializeField] CardProxy CardProxyPrefab;
    [SerializeField] PileProxy PileProxyPrefab;

    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        Actor player = new Actor(100);
        Actor enemy = new Actor(100);

        
        Battle battle = new Battle(player, new List<Actor> {enemy}, new Deck());
        GameManager.Instance.Api.SetCurrentBattle(battle);
        InitializeProxies(battle);
    }

    private void InitializeProxies(Battle battle)
    {
        ActorProxy playerProxy = Instantiate(ActorProxyPrefab);
        playerProxy.Initialize(battle.Player);

        foreach (Actor enemy in battle.Enemies)
        {
            ActorProxy enemyProxy = Instantiate(ActorProxyPrefab);
            enemyProxy.Initialize(enemy);
        }

        //piles probably shouldn't be instantiated and should instead exist in the scene and hook up.
        //Maybe they shouldn't even be proxy/entity.
        PileProxy discardProxy = Instantiate(PileProxyPrefab);
        PileProxy handProxy = Instantiate(PileProxyPrefab);
        PileProxy drawPileProxy = Instantiate(PileProxyPrefab);
        PileProxy exhaustPileProxy = Instantiate(PileProxyPrefab);

        discardProxy.Initialize(battle.Deck.DiscardPile);
        handProxy.Initialize(battle.Deck.HandPile);
        drawPileProxy.Initialize(battle.Deck.DrawPile);
        exhaustPileProxy.Initialize(battle.Deck.ExhaustPile);

        //cards are kinda weird too. I'm not sure if it makes sense to pre-create them... but maybe it does?
        //at the very least the card proxies will have to know about what pile they are in. There's not really a 
        //mechanism for that right now.
        foreach (Card card in battle.Deck.AllCards())
        {
            var cardProxy = Instantiate(CardProxyPrefab);
            cardProxy.Initialize(card);
        }
    }
}