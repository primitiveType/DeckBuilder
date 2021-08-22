﻿using System.Collections.Generic;
using Data;
using DeckbuilderLibrary.Data.GameEntities;

public interface IContext
{
    void SetCurrentBattle(IBattle battle);
    int GetPlayerHealth();
    IReadOnlyList<IActor> GetEnemies();
    IBattle GetCurrentBattle();

    IActor GetActorById(int id);
    IGameEventHandler Events { get; }
    void AddEntity(IGameEntity entity);

    void TrySendToPile(int cardId, PileType pileType);
    T CreateEntity<T>() where T : GameEntity, new();
    IDeck CreateDeck();
    IPile CreatePile();
    int GetDamageAmount(object sender, int baseDamage, IActor target, IActor owner);
    void TryDealDamage(GameEntity source, IActor owner,  IActor target, int baseDamage);
    Actor CreateActor<T>(int health, int armor) where T : Actor, new ();
    IBattle CreateBattle(IDeck deck, Actor player);
    T CreateIntent<T>(Actor owner) where T : Intent, new();
}

public interface IContextListener
{
}