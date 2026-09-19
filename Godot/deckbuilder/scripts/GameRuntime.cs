using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Godot;
using SummerJam1;
using SummerJam1.Cards.Effects;
using SummerJam1.Characters;
using Card = CardsAndPiles.Components.Card;

namespace Deckbuilder;

public enum CardTargetMode
{
    Self,
    OneEnemy,
    AllEnemies,
    RandomEnemy
}

public partial class GameRuntime : Node
{
    private readonly List<IDisposable> _eventHandles = new();
    private int _entityCreatedCount;

    public event Action<string>? MessageLogged;
    public event Action? SnapshotChanged;
    public event Action? HandChanged;

    public Context? Context { get; private set; }
    public Game? Game { get; private set; }
    public SummerJam1Events? Events => Context?.Events as SummerJam1Events;

    public bool IsBooted => Context != null && Game != null;

    public override void _Ready()
    {
        Logging.Initialize(new GodotLogger(Log));
    }

    public void Boot(string contentRoot)
    {
        ResetRuntime();

        if (!Directory.Exists(contentRoot))
        {
            throw new DirectoryNotFoundException($"Deckbuilder content root was not found: {contentRoot}");
        }

        Context = new Context(new SummerJam1Events());
        Context.SetPrefabsDirectory(contentRoot);
        SubscribeToEvents();

        Game = Context.Root.AddComponent<Game>();
        Log($"Booted Game. Content root: {contentRoot}");
        Log(GetSnapshot());
        NotifyHandChanged();
    }

    public void StartBattle()
    {
        RequireGame().StartBattle();
        Log("StartBattle invoked.");
        Log(GetSnapshot());
        NotifyHandChanged();
    }

    public void EndTurn()
    {
        RequireGame().EndTurn();
        Log("EndTurn invoked.");
        Log(GetSnapshot());
        NotifyHandChanged();
    }

    public void WaitForCard()
    {
        RequireGame().WaitForCard();
        Log("WaitForCard invoked.");
        Log(GetSnapshot());
        NotifyHandChanged();
    }

    public void CreateSmokeEntity()
    {
        var context = RequireContext();
        var entity = context.CreateEntity(context.Root, child =>
        {
            child.AddComponent<NameComponent>().Value = "Godot Smoke Entity";
            child.AddComponent<DescriptionComponent>().Description = "Created from the Godot API test scene.";
        });

        Log($"Created smoke entity #{entity.Id} with {entity.Components.Count} components.");
        Log(GetSnapshot());
        SnapshotChanged?.Invoke();
    }

    public IReadOnlyList<IEntity> GetHandCards()
    {
        return Game?.Battle?.Hand?.Entity.Children.ToList() ?? new List<IEntity>();
    }

    public IReadOnlyList<IEntity> GetEnemyUnits()
    {
        return Game?.Battle?.MonsterSlots?.Entity.Children.ToList() ?? new List<IEntity>();
    }

    public IReadOnlyList<IEntity> GetPartyMembers()
    {
        return Game?.Party?.ActiveMembers ?? new List<IEntity>();
    }

    public CardTargetMode GetCardTargetMode(IEntity card)
    {
        List<IEffect> effects = card.GetComponents<IEffect>();
        if (effects.Count == 0)
        {
            return CardTargetMode.Self;
        }

        if (effects.Any(effect => effect.Targeting == TargetingType.Unit) && card.GetComponent<Targeting>()?.Aoe != true)
        {
            return CardTargetMode.OneEnemy;
        }

        if (effects.Any(effect => effect.Targeting == TargetingType.AllEnemies) || card.GetComponent<Targeting>()?.Aoe == true)
        {
            return CardTargetMode.AllEnemies;
        }

        if (effects.Any(effect => effect.Targeting == TargetingType.RandomEnemy))
        {
            return CardTargetMode.RandomEnemy;
        }

        return CardTargetMode.Self;
    }

    public bool TryPlayCard(IEntity card, IEntity target)
    {
        var cardComponent = card.GetComponent<Card>();
        if (cardComponent == null)
        {
            Log($"Unable to play entity #{card.Id}; it has no card component.");
            return false;
        }

        bool played = cardComponent.TryPlayCard(target);
        Log(played
            ? $"Played {card.GetComponent<NameComponent>()?.Value ?? card.Id.ToString()} on {target.GetComponent<NameComponent>()?.Value ?? target.Id.ToString()}."
            : $"Failed to play {card.GetComponent<NameComponent>()?.Value ?? card.Id.ToString()}.");
        NotifyHandChanged();
        return played;
    }

    public bool TryPlayCardWithDefaultTarget(IEntity card)
    {
        IEntity? target = GetCardTargetMode(card) switch
        {
            CardTargetMode.AllEnemies or CardTargetMode.RandomEnemy => GetEnemyUnits().FirstOrDefault(),
            _ => Game?.Player?.Entity
        };

        if (target == null)
        {
            Log($"Unable to play {card.GetComponent<NameComponent>()?.Value ?? card.Id.ToString()}; no default target exists.");
            return false;
        }

        return TryPlayCard(card, target);
    }

    public string GetSnapshot()
    {
        if (Context == null || Game == null)
        {
            return "Runtime not booted.";
        }

        int deckCount = Game.Deck?.Entity.Children.Count ?? 0;
        int prefabCardCount = Game.PrefabDebugPileTester?.Entity.Children.Count ?? 0;
        int battleChildCount = Game.Battle?.Entity.Children.Count ?? 0;
        string battleState = Game.Battle == null ? "none" : $"active ({battleChildCount} children)";

        return string.Join("\n", new[]
        {
            $"Entities: {Context.EntityDatabase.Count}",
            $"Root children: {Context.Root.Children.Count}",
            $"Deck cards: {deckCount}",
            $"Prefab card tester: {prefabCardCount}",
            $"Battle: {battleState}",
            $"EntityCreated events: {_entityCreatedCount}"
        });
    }

    private void SubscribeToEvents()
    {
        var events = Events ?? throw new InvalidOperationException("Events unavailable before context boot.");

        _eventHandles.Add(events.SubscribeToEntityCreated((_, args) =>
        {
            _entityCreatedCount++;
            if (_entityCreatedCount <= 20 || _entityCreatedCount % 25 == 0)
            {
                string components = string.Join(", ", args.Entity.Components.Select(component => component.GetType().Name));
                string entityName = args.Entity.GetComponent<NameComponent>()?.Value ?? $"Entity {args.Entity.Id}";
                Log($"EntityCreated #{args.Entity.Id}: {entityName} [{components}]");
            }
        }));

        _eventHandles.Add(events.SubscribeToGameStarted((_, _) => Log("GameStarted event received.")));
        _eventHandles.Add(events.SubscribeToBattleStarted((_, _) => Log("BattleStarted event received.")));
        _eventHandles.Add(events.SubscribeToCardPlayFailed((_, args) => Log($"CardPlayFailed: {string.Join("; ", args.Reasons)}")));
        _eventHandles.Add(events.SubscribeToCardPlayed((_, args) =>
        {
            Log($"CardPlayed: entity #{args.CardId.Id} target #{args.Target?.Id.ToString() ?? "none"}");
            NotifyHandChanged();
        }));
        _eventHandles.Add(events.SubscribeToTurnBegan((_, _) =>
        {
            Log("TurnBegan event received.");
            NotifyHandChanged();
        }));
        _eventHandles.Add(events.SubscribeToCardDrawn((_, args) =>
        {
            string cardName = args.DrawnCard.GetComponent<NameComponent>()?.Value ?? $"Entity {args.DrawnCard.Id}";
            Log($"CardDrawn: {cardName} (hand draw: {args.IsHandDraw})");
            NotifyHandChanged();
        }));
        _eventHandles.Add(events.SubscribeToDamageDealt((_, _) => NotifyHandChanged()));
        _eventHandles.Add(events.SubscribeToEntityKilled((_, _) => NotifyHandChanged()));
        _eventHandles.Add(events.SubscribeToDiscardPhaseBegan((_, _) => NotifyHandChanged()));
        _eventHandles.Add(events.SubscribeToDrawPhaseBegan((_, _) => NotifyHandChanged()));
    }

    private void ResetRuntime()
    {
        foreach (var eventHandle in _eventHandles)
        {
            eventHandle.Dispose();
        }

        _eventHandles.Clear();
        _entityCreatedCount = 0;
        Game = null;
        Context = null;
    }

    private Context RequireContext()
    {
        return Context ?? throw new InvalidOperationException("Boot the deckbuilder runtime first.");
    }

    private Game RequireGame()
    {
        return Game ?? throw new InvalidOperationException("Boot the deckbuilder runtime first.");
    }

    private void Log(string message)
    {
        MessageLogged?.Invoke(message);
    }

    private void NotifyHandChanged()
    {
        SnapshotChanged?.Invoke();
        HandChanged?.Invoke();
    }
}
