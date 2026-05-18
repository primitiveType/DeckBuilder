# Godot Migration Plan

This project is moving from a Unity-hosted UI to a Godot-hosted UI while keeping the C# gameplay model as the source of truth.

## Current Boundary

The model projects are:

- `Api`: entity/component runtime, context, lifecycle, serialization, generated base events.
- `CardsAndPiles`: card, pile, deck, hand, discard, damage/heal, and turn abstractions.
- `SummerJam1`: concrete game rules, cards, units, statuses, relics, shops, rewards, and prefab content.

The old Unity project used these as compiled DLLs. Unity scripts created the model, listened to model events, instantiated Unity prefabs, and bound UI elements to `IEntity`, `IComponent`, `INotifyPropertyChanged`, and child collection changes.

The Godot port should keep that same high-level split, but it should not put engine-specific objects back into model components.

## First Technical Decisions

- Use Godot 4 .NET and target `net8.0`.
- Reference the model projects by `ProjectReference` from the Godot C# project.
- Target active model libraries at `net8.0`.
- Keep JSON prefab type names stable by preserving assembly names and component namespaces.
- Keep generated event `.cs` files checked in for now; replace T4 later with a deterministic generator/tool.
- Keep `PropertyChanged.Fody` for now to avoid changing behavior while moving engines.

## Godot Adapter Shape

Create these Godot-side services first:

- `GameRuntime : Node`
  - Godot autoload singleton.
  - Creates `Context` with `SummerJam1Events`.
  - Calls `Context.SetPrefabsDirectory(contentRoot)`.
  - Owns `Game`, save/load, and high-level scene transitions.

- `GodotLogger : Api.ILogger`
  - Sends model logging to `GD.Print`, `GD.PushWarning`, and `GD.PushError`.

- `EntityViewRegistry`
  - Maps `IEntity.Id` to Godot `Node`.
  - Replaces Unity's model-attached `SummerJam1ModelViewBridge`.
  - Keeps engine references out of serialized gameplay entities.

- `EntityViewFactory : Node`
  - Subscribes to `EntityCreated`.
  - Chooses a `PackedScene` based on `IVisual` component type.
  - Instantiates views and registers them in `EntityViewRegistry`.

- `ModelView<T> : Node`
  - Stores an `IEntity` and its component model.
  - Subscribes to entity destruction and component property changes.

- `ComponentView<T> : Node`
  - Binds one model component to one Godot scene node.
  - Updates visibility/state when components are added, removed, or changed.

- `PileView : Control` or `Node2D`
  - Subscribes to `Entity.Children.CollectionChanged`.
  - Reparents/reorders registered child views.

## Content Paths

The current content lives at:

- `SummerJam1/StreamingAssets/Prefabs`
- `SummerJam1/StreamingAssets/Resources`

For Godot, mirror or copy that content under a single root such as:

- `GodotClient/Content/Prefabs`
- `GodotClient/Content/Resources`

`Context.SetPrefabsDirectory(contentRoot)` now expands that root to `Prefabs` and `Resources`, which matches both Godot and the old Unity build-time layout.

## Initial Milestone

The first working Godot milestone should be deliberately small:

1. Start a Godot .NET project.
2. Reference `Api`, `CardsAndPiles`, and `SummerJam1`.
3. Create `GameRuntime` as an autoload.
4. Set content paths.
5. Create a new `Context` and add `Game` to the root entity.
6. Subscribe to `GameStarted`, `EntityCreated`, `BattleStarted`, and `CardPlayFailed`.
7. Print those events in the Godot output.
8. Start a battle from a button and verify model events fire.

Only after that should card visuals, drag/drop, targeting, and reward screens be ported.

## Build And Test Commands

Use the .NET 8 SDK pinned by the repository `global.json`.

```bash
dotnet restore Cards.sln
dotnet build Cards.sln --no-restore -m:1
dotnet test Cards.sln --no-build -m:1
```

The `-m:1` flag avoids an MSBuild project-reference target issue seen during the migration where parallel solution builds could fail without a useful diagnostic.

## Cleanup Backlog

- Remove Unity-only view bridge concepts from any shared model code.
- Replace T4 event generation with a source generator or explicit build tool.
- Consider replacing static `Context.PrefabsPath` and `Context.ResourcesPath` with instance properties.
- Continue improving the Godot entity authoring scene for prefab creation and editing.
- Remove stale projects and deleted Solitaire references from the active solution.
- Add a CI/build script once the .NET SDK is available in the environment.
