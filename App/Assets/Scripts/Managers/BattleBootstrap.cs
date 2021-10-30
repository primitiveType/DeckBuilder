using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] public DeckCreationPileProxy m_DeckCreationPileProxy;
    DeckCreationPileProxy DeckCreationPileProxy => m_DeckCreationPileProxy;

    [SerializeField] public DiscoverPileProxy m_DiscoverPileProxy;
    DiscoverPileProxy DiscoverPileProxy => m_DiscoverPileProxy;

    [SerializeField] public ExhaustPileProxy m_ExhaustPileProxy;
    ExhaustPileProxy ExhaustPileProxy => m_ExhaustPileProxy;

    [FormerlySerializedAs("m_PlayerProxy")] [SerializeField]
    public PlayerActorProxy m_PlayerUIProxy;

    PlayerActorProxy PlayerUIProxy => m_PlayerUIProxy;

    [SerializeField] private int m_NumCardsInTestDeck;
    [SerializeField] private Button m_EndTurnButton;
    private Button EndTurnButton => m_EndTurnButton;
    int NumCardsInTestDeck => m_NumCardsInTestDeck;

    private IContext Context => GameContextManager.Instance.Context;

    void Start()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        EndTurnButton.onClick.AddListener(Context.EndTurn);
        InitializeProxies(Context.GetCurrentBattle());

        InputManager.Instance.TransitionToState(InputState.DefaultBattle);
    }

    private void InitializeProxies(IBattle battle)
    {
        PlayerUIProxy.Initialize(battle.Player);
        DeckCreationPileProxy.Initialize(battle.Deck.DiscoverPile);

        Factory.Instance.CreateProxyForEntity(battle.Player); //creates the token.
        DiscardProxy.Initialize(battle.Deck.DiscardPile);
        HandProxy.Initialize(battle.Deck.HandPile);
        DrawPileProxy.Initialize(battle.Deck.DrawPile);
        ExhaustPileProxy.Initialize(battle.Deck.ExhaustPile);
        DiscoverPileProxy.Initialize(battle.Deck.DiscoverPile);
    }
}