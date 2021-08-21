using System.Collections.Generic;
using Content.Cards;
using Data;
using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] ActorProxy ActorProxyPrefab;
    [SerializeField] CardProxy CardProxyPrefab;


    [SerializeField] public DiscardPileProxy discardProxy;

    [SerializeField] public HandPileProxy handProxy;

    [SerializeField] public DrawPileProxy drawPileProxy;

    [SerializeField] public ExhaustPileProxy exhaustPileProxy;

    [SerializeField] public EnemyActorManager enemyActorManager;

    [SerializeField] private int NumCardsInTestDeck;

    private IContext Api { get; set; }

    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        Api = new GameContext();

        IActor player = Api.CreateActor<Actor>(100, 0);
        IActor enemy = Api.CreateActor<Actor>(100, 0);


        IDeck deck = Api.CreateDeck();


        for (int i = 0; i < NumCardsInTestDeck; i++)
        {
            if (i % 2 == 0)
            {
                deck.DrawPile.Cards.Add(Api.CreateEntity<Attack5Damage>());
            }
            else
            {
                deck.DrawPile.Cards.Add(Api.CreateEntity<Attack10DamageExhaust>());
            }

            deck.DrawPile.Cards.Add(Api.CreateEntity<DoubleNextCardDamage>());
        }

        IBattle battle = Api.CreateEntity<Battle>();
        battle.Deck = deck;
        battle.Player = player;
        battle.Enemies = new List<Actor> { enemy };
        Api.SetCurrentBattle(battle);
        InitializeProxies(battle);
    }

    private void InitializeProxies(IBattle battle)
    {
        ActorProxy playerProxy = Instantiate(ActorProxyPrefab);
        playerProxy.Initialize((Actor)battle.Player);

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