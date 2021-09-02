using Content.Cards;
using DeckbuilderLibrary.Data;
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

    private IContext Context => GameContextManager.Instance.Context;

    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        EndTurnButton.onClick.AddListener(Context.EndTurn);
        InitializeProxies(Context.GetCurrentBattle());
    }

    private void InitializeProxies(IBattle battle)
    {
        BattleProxy.Initialize(battle);

        PlayerProxy.Initialize(battle.Player);

        foreach (Actor enemy in battle.Enemies)
        {
            EnemyActorManager.CreateEnemyActor(enemy);
        }
        
        DiscardProxy.Initialize(battle.Deck.DiscardPile);
        HandProxy.Initialize(battle.Deck.HandPile);
        DrawPileProxy.Initialize(battle.Deck.DrawPile);
        ExhaustPileProxy.Initialize(battle.Deck.ExhaustPile);

     
    }
}