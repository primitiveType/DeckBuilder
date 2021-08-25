using System.Collections.Generic;
using System.Collections.Specialized;
using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using UnityEngine;
using UnityEngine.UI;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] EnemyActorProxy m_ActorProxyPrefab;
    EnemyActorProxy ActorProxyPrefab => m_ActorProxyPrefab;

    [SerializeField] public DiscardPileProxy m_DiscardProxy;
    DiscardPileProxy DiscardProxy => m_DiscardProxy;

    [SerializeField] public HandPileProxy m_HandProxy;
    HandPileProxy HandProxy => m_HandProxy;

    [SerializeField] public DrawPileProxy m_DrawPileProxy;
    DrawPileProxy DrawPileProxy => m_DrawPileProxy;

    [SerializeField] public ExhaustPileProxy m_ExhaustPileProxy;
    ExhaustPileProxy ExhaustPileProxy => m_ExhaustPileProxy;
    [SerializeField] public BattleProxy m_BattleProxy;
    BattleProxy BattleProxy => m_BattleProxy;
    [SerializeField] public PlayerActorProxy m_PlayerProxy;
    PlayerActorProxy PlayerProxy => m_PlayerProxy;

    [SerializeField] public EnemyActorManager m_EnemyActorManager;
    EnemyActorManager EnemyActorManager => m_EnemyActorManager;

    [SerializeField] private int m_NumCardsInTestDeck;
    [SerializeField] private Button m_EndTurnButton;
    private Button EndTurnButton => m_EndTurnButton;
    int NumCardsInTestDeck => m_NumCardsInTestDeck;

    private IContext Api { get; set; }

    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        Api = new GameContext();

        EndTurnButton.onClick.AddListener(EndTurn);
        PlayerActor player = Api.CreateActor<PlayerActor>(100, 0);
        Actor enemy = Api.CreateActor<BasicEnemy>(100, 0);
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

        IBattle battle = Api.CreateBattle(deck, player);
        battle.AddEnemy(enemy);
        Api.SetCurrentBattle(battle);
        InitializeProxies(battle);
    }

    private void EndTurn()
    {
        Api.Events.InvokeTurnEnded(this, new TurnEndedEventArgs());
    }

    private void InitializeProxies(IBattle battle)
    {
        BattleProxy.Initialize(battle);

        PlayerProxy.Initialize(battle.Player);

        foreach (Actor enemy in battle.Enemies)
        {
            EnemyActorManager.CreateEnemyActor(enemy);
        }

        //piles probably shouldn't be instantiated and should instead exist in the scene and hook up.
        //Maybe they shouldn't even be proxy/entity.
        //PileProxy discardProxy = Instantiate(PileProxyPrefab);
        //PileProxy handProxy = Instantiate(PileProxyPrefab);
        //PileProxy drawPileProxy = Instantiate(PileProxyPrefab);
        //PileProxy exhaustPileProxy = Instantiate(PileProxyPrefab);

        DiscardProxy.Initialize(battle.Deck.DiscardPile);
        HandProxy.Initialize(battle.Deck.HandPile);
        DrawPileProxy.Initialize(battle.Deck.DrawPile);
        ExhaustPileProxy.Initialize(battle.Deck.ExhaustPile);

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