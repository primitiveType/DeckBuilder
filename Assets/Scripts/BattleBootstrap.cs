﻿using System.Collections.Generic;
using Data;
using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] ActorProxy ActorProxyPrefab;
    [SerializeField] CardProxy CardProxyPrefab;
   // [SerializeField] PileProxy PileProxyPrefab;

    [SerializeField]
    public GenericPileProxy discardProxy;

    [SerializeField]
    public HandPileProxy handProxy;

    [SerializeField]
    public GenericPileProxy drawPileProxy;

    [SerializeField]
    public GenericPileProxy exhaustPileProxy;

    [SerializeField]
    public EnemyActorManager enemyActorManager;

    [SerializeField]
    private int NumCardsInTestDeck;
    private IGlobalApi Api => Injector.GlobalApi;
    
    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        Actor player = new Actor(100);
        Actor enemy = new Actor(100);

        Deck deck = new Deck();
        Api.AddCard(TestCards.Attack5Damage, nameof(TestCards.Attack5Damage));
        for(int i = 0; i < NumCardsInTestDeck; i++)
        {
            deck.DrawPile.Add(Api.CreateCardInstance(nameof(TestCards.Attack5Damage)));
        }

        Battle battle = new Battle(player, new List<Actor> {enemy}, deck);
        Api.SetCurrentBattle(battle);
        InitializeProxies(battle);
    }

    private void InitializeProxies(Battle battle)
    {
        ActorProxy playerProxy = Instantiate(ActorProxyPrefab);
        playerProxy.Initialize(battle.Player);

        foreach (Actor enemy in battle.Enemies)
        {
            enemyActorManager.CreateEnemyActor(enemy);
        }

        //piles probably shouldn't be instantiated and should instead exist in the scene and hook up.
        //Maybe they shouldn't even be proxy/entity.
        //PileProxy discardProxy = Instantiate(PileProxyPrefab);
        //PileProxy handProxy = Instantiate(PileProxyPrefab);
        //PileProxy drawPileProxy = Instantiate(PileProxyPrefab);
        //PileProxy exhaustPileProxy = Instantiate(PileProxyPrefab);

        discardProxy.Initialize(battle.Deck.DiscardPile);
        handProxy.Initialize(battle.Deck.HandPile);
        drawPileProxy.Initialize(battle.Deck.DrawPile);
        exhaustPileProxy.Initialize(battle.Deck.ExhaustPile);

        //cards are kinda weird too. I'm not sure if it makes sense to pre-create them... but maybe it does?
        //at the very least the card proxies will have to know about what pile they are in. There's not really a 
        //mechanism for that right now.
        //foreach (Card card in battle.Deck.AllCards())
        //{
        //    CardProxy cardProxy = Instantiate(CardProxyPrefab);
        //    cardProxy.Initialize(card);
        //}
    }
}