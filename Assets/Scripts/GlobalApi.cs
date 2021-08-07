using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using MoonSharp.Interpreter;
using UnityEngine;

public class GlobalApi : IGlobalApi
{
    public GlobalApi()
    {
        Initialize();
    }

    private string CardsPath => Path.Combine(Application.streamingAssetsPath, "CardScripts");
    private string LuaExtension => ".lua";
    private Dictionary<string, Script> CardsByName = new Dictionary<string, Script>();
    private Battle CurrentBattle { get; set; }
    private Dictionary<int, GameEntity> EntitiesById = new Dictionary<int, GameEntity>();

    private void Log(string str, Script script = null)
    {
        Debug.Log($"Lua : {str}");
    }

    private void LoadAllCardPrototypes()
    {
        DirectoryInfo cardsDirectory = new DirectoryInfo(CardsPath);

        LoadRecursively(cardsDirectory);

        void LoadRecursively(DirectoryInfo dir)
        {
            //TODO: look into moonsharp script loaders. We should be able to just provide a path instead of a string.
            foreach (DirectoryInfo childDir in dir.EnumerateDirectories())
            {
                LoadRecursively(childDir);
            }

            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (file.Extension.ToLower() == LuaExtension)
                {
                    string scriptString = File.ReadAllText(file.FullName);
                    AddCard(scriptString, file.Name);
                }
            }
        }
    }


    private void AddCard(string scriptString, string name)
    {
        var script = new Script();
        script.DoString(scriptString);
        InitializeScriptWithGlobalApi(script);

        CardsByName.Add(name, script);
    }

    public int Mul(int a, int b, Script script = null)
    {
        return a * b;
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

    private void InitializeScriptWithGlobalApi(Script script)
    {
        script.Globals[nameof(Log)] = (Action<string, Script>) Log;
        script.Globals[nameof(Mul)] = (Func<int, int, Script, int>) Mul;
        script.Globals[nameof(GetEnemyIds)] = (Func<Script, IReadOnlyList<int>>) GetEnemyIds;
        script.Globals[nameof(DamageTarget)] = (Action<int, int, Script>) DamageTarget;
        script.Globals[nameof(SendToDiscard)] = (Action<int, Script>) SendToDiscard;
        script.Globals[nameof(SendToDraw)] = (Action<int, Script>) SendToDraw;
        script.Globals[nameof(SendToExhaust)] = (Action<int, Script>) SendToExhaust;
        script.Globals[nameof(SendToHand)] = (Action<int, Script>) SendToHand;
    }

    private void SendToDiscard(int cardId, Script script = null)
    {
        SendToPile(cardId, CardPile.DiscardPile, script);
    }


    private void SendToExhaust(int cardId, Script script = null)
    {
        SendToPile(cardId, CardPile.ExhaustPile, script);
    }


    private void SendToDraw(int cardId, Script script = null)
    {
        SendToPile(cardId, CardPile.DrawPile, script);
    }

    private void SendToHand(int cardId, Script script = null)
    {
        SendToPile(cardId, CardPile.HandPile, script);
    }

    private void SendToPile(int cardId, CardPile pile, Script script = null)
    {
        if (!EntitiesById.TryGetValue(cardId, out GameEntity entity))
        {
            throw new ArgumentException($"Failed to find card with id {cardId}!");
        }

        if (entity is Card card)
        {
            CurrentBattle.Deck.SendToPile(card, pile);
        }
        else
        {
            throw new ArgumentException($"Tried to move entity with id {cardId} but it was not a card!");
        }
    }

    private void DamageTarget(int target, int damage, Script script = null)
    {
        GetActorById(target).TryDealDamage(damage, out int totalDamage, out int healthDamage);
        //Notify the card that it dealt damage in case it cares. This should probably notify all cards that something dealt damage.
        //That will have to be moved somewhere else though. 
        script?.Call(script?.Globals["onDamageDealt"], target, totalDamage, healthDamage);
    }


    public void Initialize()
    {
        LoadAllCardPrototypes();
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

    public IReadOnlyList<Actor> GetEnemies(Script script = null)
    {
        return CurrentBattle.Enemies;
    }

    public IReadOnlyList<int> GetEnemyIds(Script script = null)
    {
        return CurrentBattle.Enemies.Select(enemy => enemy.Id).ToList();
    }

    void IGlobalApi.AddCard(string scriptString, string name) => AddCard(scriptString, name);

    public Card CreateCardInstance(string cardName, DynValue opaqueData = null)
    {
        bool found = CardsByName.TryGetValue(cardName, out Script script);
        if (!found)
        {
            throw new ArgumentException($"Card with name {cardName} not found in database!");
        }

        Card card = new Card(script, cardName, this, opaqueData);
        EntitiesById.Add(card.Id, card);

        return card;
    }
}