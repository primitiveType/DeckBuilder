using System;
using System.Collections.Generic;
using System.Linq;
using Data;
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

    [JsonProperty] private Battle CurrentBattle { get; set; }
    [JsonIgnore] private Dictionary<int, GameEntity> EntitiesById = new Dictionary<int, GameEntity>();

    public IGameEventHandler Events { get; } = new GameEventHandler();
    public static IContext CurrentContext { get; set; }

    public void AddEntity(GameEntity entity)
    {
        EntitiesById.Add(entity.Id, entity);
    }

    public Actor GetActorById(int id)
    {
        return AllActors().First(actor => actor.Id == id);
    }

    private IEnumerable<Actor> AllActors()
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
        if (!EntitiesById.TryGetValue(cardId, out GameEntity entity))
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


    public void SetCurrentBattle(Battle battle)
    {
        CurrentBattle = battle;
    }


    public Battle GetCurrentBattle()
    {
        return CurrentBattle;
    }

    public int GetPlayerHealth()
    {
        return CurrentBattle.Player.Health;
    }

    public IReadOnlyList<Actor> GetEnemies()
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

    public T CreateEntity<T>() where T : GameEntity, new()
    {
        T entity = new T
        {
             Id = m_NextId++,
            Context = this
        };
        
        EntitiesById.Add(entity.Id, entity);
        entity.Initialize();
        return entity;
    }
}