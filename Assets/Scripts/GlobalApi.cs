using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

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
        Injector.GameEventHandler.InvokeDamageDealt(this, new DamageDealtArgs(target, totalDamage, healthDamage));
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

    public IReadOnlyList<Actor> GetEnemies()
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

        Card card = new Card(cardName, opaqueData?.CastToNumber());
        card.InitializeScript(script);
        EntitiesById.Add(card.Id, card);

        return card;
    }

    public Card LoadCardFromJson(string cardStr)
    {
        Card cardCopy = JsonConvert.DeserializeObject<Card>(cardStr);
        cardCopy.InitializeScript(CardsByName[cardCopy.Name]);
        EntitiesById.Add(cardCopy.Id, cardCopy);
        return cardCopy;
    }
}

public class GameEventHandler : IGameEventHandler
{
    public event CardMovedEvent CardMoved;

    public void InvokeCardMoved(object sender, CardMovedEventArgs args)
    {
        CardMoved?.Invoke(sender, args);
    }

    public event CardPlayedEvent CardPlayed;

    public void InvokeCardPlayed(object sender, CardPlayedEventArgs args)
    {
        CardPlayed?.Invoke(sender, args);
    }

    public event CardCreatedEvent CardCreated;

    public void InvokeCardCreated(object sender, CardCreatedEventArgs args)
    {
        CardCreated?.Invoke(sender, args);
    }

    public event DamageDealt DamageDealt;

    public void InvokeDamageDealt(object sender, DamageDealtArgs args)
    {
        DamageDealt?.Invoke(sender, args);
    }
}

public delegate void DamageDealt(object sender, DamageDealtArgs args);

public class DamageDealtArgs
{
    public int ActorId { get; }
    public int HealthDamage { get; }
    public int TotalDamage { get; }

    public DamageDealtArgs(int actorId, int totalDamage, int healthDamage)
    {
        ActorId = actorId;
        HealthDamage = healthDamage;
        TotalDamage = totalDamage;
    }
}

public delegate void CardCreatedEvent(object sender, CardCreatedEventArgs args);

public class CardCreatedEventArgs
{
    public CardCreatedEventArgs(int cardId)
    {
        CardId = cardId;
    }

    public int CardId { get; }
}

public static class TestCards
{
    //If a function doesn't exist on the lua side it will throw errors when we attempt to call it.
    //So we declare all functions in a template and just append the scripts to it. We will do something similar for real cards.
    //re-declaring a function in lua is legal and just overrides it.
    //This also serves as a nice one-stop location to see what calls a lua script can implement.
    private const string BaseCardTemplate =
        @"
        instances = {}
        function getValidTargets(cardId) end
        function playCard(cardId, target) end
        function onDamageDealt(cardId, target, totalDamage, healthDamage) end
        function log(cardId) end
        function onCardPlayed(cardId) end
        function cardInstanceCreate(cardId, saveData) 
             if saveData == nil then
                Log('created instance ' .. cardId .. ' with nil data ')
                instances[cardId] = 1
            else
                Log('created instance ' .. cardId .. ' with data ' .. saveData)
                instances[cardId] = saveData
            end
            
        end
        function getCardData(cardId) end
        function onCardCreated(cardId) end
        function onCardMoved(cardId) end
        function onCardPlayed(cardId) end
          ";

    //Declaring these test cards here, so it doesn't show up in game, or randomly change, breaking tests.
    //They might need to be updated if changes to the api happen though.
    public const string Attack5Damage = BaseCardTemplate +
                                        @"function getValidTargets (cardId)
                                            return GetEnemyIds()--Basic attack that considers any enemy a valid target 
                                          end

                                          function playCard(cardId, target)
                                            DamageTarget(target, 5)
                                          end

                                           function onDamageDealt(cardId, target, totalDamage, healthDamage)
                                                if instances[cardId] != nill then
                                                    Log('dealt ' .. totalDamage .. ' damage.')
                                                end
                                            end
                                          
                                            function onCardPlayed (cardId)
                                              if instances[cardId] != nil then
                                                SendToDiscard(cardId)
                                              end
                                            end
                                ";

    public const string Attack10DamageExhaust = BaseCardTemplate +
                                                @"function getValidTargets (cardId)
                                                    return GetEnemyIds()--Basic attack that considers any enemy a valid target 
                                                  end

                                                  function playCard(cardId, target)
                                                    DamageTarget(target, 10)
                                                  end

                                                   function onDamageDealt(cardId, target, totalDamage, healthDamage)
                                                    Log('dealt ' .. totalDamage .. ' damage.')
                                                    end
                                                  
                                                    function onCardPlayed (cardId)
                                                        if instances[cardId] != nil then
                                                          SendToExhaust(cardId)
                                                        end
                                                    end
                                        ";

    public const string DealMoreDamageEachPlay = BaseCardTemplate +
                                                 @"
                                                   

                                                    
                                                  function getCardData(cardId)
                                                    return instances[cardId]
                                                  end
                                          
                                                  function getValidTargets (cardId)
                                                    return GetEnemyIds()--Basic attack that considers any enemy a valid target 
                                                  end

                                                  function playCard(cardId, target)
                                                    DamageTarget(target, instances[cardId])
                                                    instances[cardId] = instances[cardId] + 1;
                                                    Log(instances[cardId])
                                                  end

                                                   function onDamageDealt(cardId, target, totalDamage, healthDamage)
                                                    if instances[cardId] != nil then
                                                        Log('dealt ' .. totalDamage .. ' damage.')
                                                    end
                                                    end
                                                  
                                                    function onCardPlayed (cardId)
                                                        if instances[cardId] != nil then
                                                          SendToExhaust(cardId)
                                                        end
                                                    end

                                                    
                                        ";
}