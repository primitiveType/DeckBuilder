# DeckBuilder Cards

This repository contains the non-Unity C# gameplay model for a deck-building game prototype. The core code is designed to run headlessly as plain .NET libraries, with Unity or other UI layers treated as clients of the model rather than as the place where game rules live.

The main idea is composition over inheritance: game objects are entities, entities own small components, and components communicate through explicit events. Cards, units, piles, relics, rules, and status effects are all built out of the same entity/component primitives and can be serialized as JSON prefabs.

## Design Philosophy

The project keeps gameplay logic portable. Most of the important code targets `netstandard2.0`, so battle setup, card effects, event handling, prefab loading, and tests do not depend on Unity APIs. Visuals are represented through small marker/data components such as `IVisual` and `VisualComponent`; rendering is expected to happen elsewhere.

Content is data-driven. A card like `Dagger` is not a single hard-coded class; it is a JSON entity composed from `PlayerCard`, `DamageUnitCard`, `EnergyCost`, `Discard`, `NameComponent`, visual data, and other small components. This makes authoring new content mostly a matter of choosing existing components and tuning their serialized properties.

Behavior is event-oriented. Components subscribe to generated events with attributes such as `[OnCardPlayed]`, `[OnDamageDealt]`, `[OnTurnBegan]`, or `[OnBattleEnded]`. That lets rules and reactive effects live beside the component they belong to instead of being centralized in a large turn manager.

Rules are regular components. Battle-wide behavior such as drawing a hand, discarding at the right phase, or ending a battle when enemies are defeated is installed on the root entity as components. This keeps global rules testable and replaceable in the same way as card or unit behavior.

The runtime favors inspectable state. Entities have stable IDs, parent/child relationships, component collections, JSON serialization, and property change notifications. This supports tests, save/load experiments, prefab editing, and UI binding without needing the game to be running inside a specific engine.

## Architecture

### `Api`

`Api` is the lowest-level runtime. It defines the entity/component system, lifecycle, event base classes, serialization helpers, logging, random number support, and editor-facing property proxy utilities.

The important types are:

- `Context`: owns the root entity, entity ID database, prefab/resource paths, and the active event bus.
- `Entity`: an internal implementation of `IEntity` with children, parent constraints, component lookup, creation, destruction, and initialization.
- `Component`: the base class for behavior and state. During initialization it scans its methods for generated event attributes and stores disposable event handles.
- `ChildrenCollection<T>` and `EntityCollection`: observable collections used for components and child entities.
- `Serializer`: JSON.NET serialization with type names, ID-stripping support for prefabs, and converters for unknown or non-serializable components.
- `EventsBase`: generated from `EventDefinitions.xml` and extended by higher-level event catalogs.

Entity parenting is not just hierarchy; it is also validation. Components can implement `IParentConstraint` to decide whether an entity can be placed under another entity. Piles use this to accept only cards or other allowed game objects.

### Generated Events

Events are declared in XML and generated into strongly typed event buses and attributes.

- `Api/EventDefinitions.xml` defines engine-level events such as entity creation.
- `CardsAndPiles/EventDefinitions.xml` adds card, damage, heal, turn, draw, and discard events.
- `SummerJam1/EventDefinitions.xml` adds game-specific battle, unit, shop, movement, and phase events.

Each generated event has:

- an event args type,
- an `On...` method used to publish the event,
- a `SubscribeTo...` method used internally,
- an `[On...]` attribute that components place on handler methods.

The event handle wrapper catches handler exceptions and supports safe detachment. Components also track event re-entrancy per generated attribute so recursive event paths are blocked rather than exploding the stack.

### `CardsAndPiles`

`CardsAndPiles` is the reusable card-game layer on top of `Api`. It introduces cards, piles, deck/hand/discard behavior, pile constraints, card events, and simple shared components such as names, descriptions, draggable state, positions, and damage/healing interfaces.

Core concepts:

- `Pile`: a component that controls what children an entity may contain.
- `DeckPile`: draws random cards, shuffling discard back into the deck when needed.
- `HandPile`, `PlayerDeck`, `PlayerDiscard`, `PlayerExhaust`, and other pile types: named gameplay locations.
- `Card`: an abstract playable item. `TryPlayCard` asks the event system for blockers, invokes the card implementation, moves the card out of hand, and publishes `CardPlayed`.
- `CardEvents`: the event bus used by the shared card layer.

This layer intentionally does not know about a specific game mode. It provides vocabulary for deck-like games, while `SummerJam1` defines the actual game.

### `SummerJam1`

`SummerJam1` is the concrete deck-builder implementation. It references both `Api` and `CardsAndPiles` and defines the current game's cards, units, statuses, relics, objectives, battle flow, shops, rewards, and rules.

Important runtime types:

- `Game`: root gameplay coordinator. It installs rules, creates the player, deck, reward piles, relic piles, prefab catalog, map choices, shops, and battles.
- `BattleContainer`: owns battle-local piles such as battle deck, hand, discard, exhaust, monster slots, and objectives.
- `PlayerCard`: plays by running every `IEffect` component attached to the same entity.
- `Unit`: base for units and publishes `UnitCreated` when initialized.
- `Health`, `Armor`, `Strength`, `Money`, `Food`, and other stat components: small stateful components that publish or respond to events.
- `Rules/*`: root-level systems such as `DrawHandOnTurnBegin`, `DiscardHandOnDiscardPhase`, and `BattleEndsWhenAllEnemiesDefeated`.
- `Cards/Effects/*`, `Units/Effects/*`, `Statuses/*`, and `Relics/*`: modular behavior components used by JSON prefabs.

The game-specific event bus, `SummerJam1Events`, inherits from `CardsAndPiles.CardEvents`, so all card-layer events and game-layer events are available through a single context.

### JSON Prefabs

Prefab files live under `SummerJam1/StreamingAssets/Prefabs`. Each prefab is a serialized `Api.Entity` with a `ComponentsInternal` array. Components are serialized with JSON.NET type names, so a prefab can combine components from `Api`, `CardsAndPiles`, and `SummerJam1`.

For example, a card prefab may include:

- `CharacterConstraint` to restrict it to a class,
- `PlayerCard` to make it playable,
- one or more `IEffect` components to define what it does,
- `EnergyCost`, `Discard`, `StartingCard`, or other rule components,
- `NameComponent`, `Description` providers, tooltips, and visual metadata.

`Context.CreateEntity(parent, prefabName)` loads a prefab, assigns it a source reference, initializes it into the active context, optionally parents it, and publishes entity creation.

Unknown component handling is deliberately forgiving. If a serialized component type can no longer be loaded, deserialization can fall back to `UnknownComponent`; the cleanup tool can then remove unknown components and re-save prefabs.

### Tooling And Tests

The project includes several non-Unity support projects:

- `SummerJam1Tests`: NUnit tests for prefab loading, card metadata, damage, serialization experiments, event detachment, re-entrancy, and battle setup assumptions.
- `CardTestProject`: small fixtures and tests around card events and event-attribute wiring.
- `Godot/deckbuilder/scenes/entity_authoring_scene.tscn`: portable prefab authoring UI for creating, loading, editing, and saving entity JSON.
- `CleanupJson`: a console utility that loads every prefab, removes `UnknownComponent`, and serializes the cleaned entity back to disk.

These projects reinforce the main architectural goal: gameplay content and rules should be loadable, testable, and editable without requiring Unity.

## Project Layout

- `Api/`: entity/component runtime, context, generated base events, serialization, logging, and editor binding helpers.
- `CardsAndPiles/`: reusable card and pile abstractions plus card-game event definitions.
- `SummerJam1/`: concrete game implementation, rules, effects, statuses, relics, units, rewards, and JSON content.
- `SummerJam1/StreamingAssets/Prefabs/`: serialized entity prefabs for cards, units, relics, objectives, player setup, and battles.
- `SummerJam1Tests/`: NUnit coverage for the concrete game.
- `CardTestProject/`: focused card/event fixtures.
- `CleanupJson/`: prefab maintenance utility.

## Mental Model For Adding Features

1. Add state or behavior as a small component.
2. If behavior reacts to the world, subscribe with a generated event attribute.
3. If behavior affects the world, publish through the active event bus or call the narrow interface on the target component.
4. Compose the component into JSON prefabs instead of hard-coding new content where practical.
5. Add or update a focused NUnit test when the behavior changes rules, serialization, prefab validity, or event flow.

That workflow keeps the project flexible: new cards and units are usually data composition, new mechanics are usually components plus events, and engine/UI boundaries stay clean.
