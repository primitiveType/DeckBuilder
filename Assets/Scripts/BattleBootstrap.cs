using System.Collections.Generic;
using Data;
using UnityEngine;

public class BattleBootstrap : MonoBehaviour
{
    [SerializeField] ActorView m_ActorViewPrefab;
    [SerializeField] CardView m_CardViewPrefab;
    [SerializeField] PileView m_PileViewPrefab;
    private IContext Api { get; set; }
    
    void Awake()
    {
        //TODO: Set up the scene with data injected from elsewhere.
        Actor player = new Actor(100, Api);
        Actor enemy = new Actor(100, Api);

        Api = new GameContext();
        Battle battle = new Battle(player, new List<Actor> {enemy}, new Deck(Api), Api);
        Api.SetCurrentBattle(battle);
        InitializeProxies(battle);
    }

    private void InitializeProxies(Battle battle)
    {
        ActorView playerView = Instantiate(m_ActorViewPrefab);
        playerView.Initialize(battle.Player);

        foreach (Actor enemy in battle.Enemies)
        {
            ActorView enemyView = Instantiate(m_ActorViewPrefab);
            enemyView.Initialize(enemy);
        }

        //piles probably shouldn't be instantiated and should instead exist in the scene and hook up.
        //Maybe they shouldn't even be proxy/entity.
        PileView discardView = Instantiate(m_PileViewPrefab);
        PileView handView = Instantiate(m_PileViewPrefab);
        PileView drawPileView = Instantiate(m_PileViewPrefab);
        PileView exhaustPileView = Instantiate(m_PileViewPrefab);

        discardView.Initialize(battle.Deck.DiscardPile);
        handView.Initialize(battle.Deck.HandPile);
        drawPileView.Initialize(battle.Deck.DrawPile);
        exhaustPileView.Initialize(battle.Deck.ExhaustPile);

        //cards are kinda weird too. I'm not sure if it makes sense to pre-create them... but maybe it does?
        //at the very least the card proxies will have to know about what pile they are in. There's not really a 
        //mechanism for that right now.
        foreach (Card card in battle.Deck.AllCards())
        {
            CardView cardView = Instantiate(m_CardViewPrefab);
            cardView.Initialize(card);
        }
    }
}