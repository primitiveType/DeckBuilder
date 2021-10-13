using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DeckCreationBootstrap : MonoBehaviour
{

    [SerializeField] public DeckCreationPileProxy m_DeckCreationPileProxy;
    DeckCreationPileProxy DeckCreationPileProxy => m_DeckCreationPileProxy;

    private IContext Context => GameContextManager.Instance.Context;

    void Awake()
    {
        InitializeProxies(Context.GetCurrentBattle());

        InputManager.Instance.TransitionToState(InputState.DeckCreation);

        List<Type> AllCards = new List<Type>();

        foreach (Type type in Assembly.GetAssembly(typeof(EnergyCard)).GetTypes().AsEnumerable().Where(x => typeof(Card).IsAssignableFrom(x)))
        {
            if (!type.IsAbstract)
            {
                AllCards.Add(type);
            }

        }

        //For now, PileType.None is used to denote that cards should be added to the PlayerDeck. 
        Context.Discover(AllCards, PileType.None);

      
        
        
    }

    private void InitializeProxies(IBattle battle)
    {

        DeckCreationPileProxy.Initialize(battle.Deck.DiscoverPile);
    }
}
