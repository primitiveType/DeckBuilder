using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using Newtonsoft.Json;
using UnityEngine;


[MoonSharpUserData]
public class GameContext : ITestContext
{
    private CardsDatabase CardsDatabase;

    static GameContext()
    {
        UserData.RegisterAssembly();
    }

    [JsonConstructor]
    public GameContext()
    {
        CurrentContext = this;
        CardsDatabase = new CardsDatabase(this);
    }

    [JsonProperty] private Battle CurrentBattle { get; set; }
    private SerializableDictionary<int, GameEntity> EntitiesById = new SerializableDictionary<int, GameEntity>();

    public IGameEventHandler Events { get; } = new GameEventHandler();
    public static IContext CurrentContext { get; private set; }
    
    public void AddEntity(GameEntity entity)
    {
        EntitiesById.Add(entity.Id, entity);
    }

    [MoonSharpVisible(true)]
    private void Log(string str, Script script = null)
    {
        Debug.Log($"Lua : {str}");
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


    [MoonSharpVisible(true)]
    private void SendToDiscard(int cardId, Script script = null)
    {
        SendToPile(cardId, PileType.DiscardPile, script);
    }


    [MoonSharpVisible(true)]
    private void SendToExhaust(int cardId, Script script = null)
    {
        SendToPile(cardId, PileType.ExhaustPile, script);
    }


    [MoonSharpVisible(true)]
    private void SendToDraw(int cardId, Script script = null)
    {
        SendToPile(cardId, PileType.DrawPile, script);
    }

    [MoonSharpVisible(true)]
    private void SendToHand(int cardId, Script script = null)
    {
        SendToPile(cardId, PileType.HandPile, script);
    }

    [MoonSharpVisible(true)]
    private int GetInt(int cardId, string key, Script script = null)
    {
        int answer = 0;
        if (EntitiesById.ContainsKey(cardId))
        {
            EntitiesById[cardId].Properties.Ints.TryGetValue(key, out int value);
            answer = value;
        }

        return answer;
    }

    [MoonSharpVisible(true)]
    private void SetInt(int cardId, string key, int value, Script script = null)
    {
        var card = EntitiesById[cardId];

        card.Properties.Ints[key] = value;
    }

    [MoonSharpVisible(true)]
    private void SendToPile(int cardId, PileType pileType, Script script = null)
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

    [MoonSharpVisible(true)]
    private void DamageTarget(int target, int damage, Script script = null)
    {
        GetActorById(target).TryDealDamage(damage, out int totalDamage, out int healthDamage);
        Events.InvokeDamageDealt(this, new DamageDealtArgs(target, totalDamage, healthDamage));
    }
    //
    // [OnDeserialized]
    // private void OnDeserialized(StreamingContext streamingContext)
    // {
    //     (CurrentBattle as IInitializeApi).InitializeContext(this);
    // }


    public void AddCard(string scriptString, string name)
    {
        CardsDatabase.AddCardScript(scriptString, name); //test purposes only
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

    public IReadOnlyList<int> GetEnemyIds(Script script = null)
    {
        return CurrentBattle.Enemies.Select(enemy => enemy.Id).ToList();
    }


    public Card CreateCardInstance(string cardName)
    {
        Script script = CardsDatabase.GetCardScript(cardName);

        Card card = new Card(cardName, this);
        card.InitializeScript(script);
        return card;
    }

    public Script GetCardScript(string name)
    {
        return CardsDatabase.GetCardScript(name);
    }

    private int m_NextId { get; set; }

    public int GetNextEntityId()
    {
        return m_NextId++;
    }
}