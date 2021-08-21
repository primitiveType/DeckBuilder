using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using Newtonsoft.Json;


public class GameContext : ITestContext
{
    private CardsDatabase CardsDatabase;

    [JsonConstructor]
    public GameContext()
    {
        CurrentContext = this;
        CardsDatabase = new CardsDatabase(this);
    }

    [JsonProperty] private IBattle CurrentBattle { get; set; }
    [JsonIgnore] private Dictionary<int, IGameEntity> EntitiesById = new Dictionary<int, IGameEntity>();

    public IGameEventHandler Events { get; } = new GameEventHandler();
    public static IContext CurrentContext { get; set; }

    public void AddEntity(IGameEntity entity)
    {
        EntitiesById.Add(entity.Id, entity);
    }

    public IActor GetActorById(int id)
    {
        return AllActors().First(actor => actor.Id == id);
    }

    private IEnumerable<IActor> AllActors()
    {
        yield return CurrentBattle.Player;
        foreach (var actor in CurrentBattle.Enemies)
        {
            yield return actor;
        }
    }


    private void SendToDiscard(int cardId)
    {
        SendToPile(cardId, PileType.DiscardPile);
    }


    private void SendToExhaust(int cardId)
    {
        SendToPile(cardId, PileType.ExhaustPile);
    }

    private void SendToDraw(int cardId)
    {
        SendToPile(cardId, PileType.DrawPile);
    }

    private void SendToHand(int cardId)
    {
        SendToPile(cardId, PileType.HandPile);
    }


    public void SendToPile(int cardId, PileType pileType)
    {
        if (!EntitiesById.TryGetValue(cardId, out IGameEntity entity))
        {
            throw new ArgumentException($"Failed to find card with id {cardId}!");
        }

        if (entity is Card card)
        {
            CurrentBattle.Deck.SendToPile(card, pileType);
        }
        else
        {
            throw new ArgumentException($"Tried to move entity with id {cardId} but it was not a card!");
        }
    }


    public void SetCurrentBattle(IBattle battle)
    {
        CurrentBattle = battle;
    }


    public IBattle GetCurrentBattle()
    {
        return CurrentBattle;
    }

    public int GetPlayerHealth()
    {
        return CurrentBattle.Player.Health;
    }

    public IReadOnlyList<IActor> GetEnemies()
    {
        return CurrentBattle.Enemies;
    }

    public IReadOnlyList<int> GetEnemyIds()
    {
        return CurrentBattle.Enemies.Select(enemy => enemy.Id).ToList();
    }


    private int m_NextId { get; set; }

    private int GetNextEntityId()
    {
        return m_NextId++;
    }

    public int GetDamageAmount(object sender, int baseDamage, IActor target, IActor owner)
    {
        return ((IInternalGameEventHandler)Events).RequestDamage(sender, baseDamage, target);
    }

    public void TryDealDamage(GameEntity source, IActor owner, IActor target, int baseDamage)
    {
        ((IInternalActor)target).TryDealDamage(source, GetDamageAmount(source, baseDamage, target, owner),
            out int totalDamageDealt, out int healthDamageDealt);
    }

    public T CreateEntity<T>() where T : GameEntity, new()
    {
        T entity = new T
        {
            Id = m_NextId++,
        };
        entity.SetContext(this);
        EntitiesById.Add(entity.Id, entity);
        ((IInternalGameEntity)entity).InternalInitialize();
        return entity;
    }

    public IDeck CreateDeck()
    {
        return CreateEntity<Deck>();
    }

    public IPile CreatePile()
    {
        return CreateEntity<Pile>();
    }

    public T CreateIntent<T>(Actor owner) where T : Intent, new()
    {
        var intent = CreateEntity<T>();
        intent.OwnerId = owner.Id;
        return intent;
    }

    public Actor CreateActor<T>(int health, int armor) where T : Actor, new ()
    {
        var actor = (Actor)CreateEntity<T>();
        actor.Health = health;
        actor.Armor = armor;
        
        return actor;
    }

    public IBattle CreateBattle(IDeck deck, Actor player)
    {
        var battle = CreateEntity<Battle>();
        battle.SetDeck(deck);
        battle.SetPlayer(player);

        return battle;
    }
}