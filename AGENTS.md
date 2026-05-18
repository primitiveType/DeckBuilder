# Project Instructions

## Repository Shape

- `Cards/` contains the portable C# gameplay model and tests.
- `Godot/deckbuilder/` contains the Godot 4.6 C# client and tools.
- Gameplay rules should stay in the C# model under `Cards/` unless a change is strictly presentation or tooling.
- Godot code should act as a client of the model, not as the source of game rules.

## Build And Test

- Build the gameplay libraries and tests from `Cards/`:

```bash
dotnet build Cards.sln -m:1
dotnet test Cards.sln -m:1
```

- Build the Godot project from `Godot/deckbuilder/`:

```bash
dotnet build Deckbuilder.csproj
```

- Use `-m:1` for solution-level builds/tests unless the project-reference build issue has been fixed.

## Content And Prefabs

- Entity prefabs live under `Cards/SummerJam1/StreamingAssets/Prefabs`.
- Prefabs are serialized `Api.Entity` JSON with component type names.
- Prefer composing new content from existing components before adding new gameplay classes.
- Use `Serializer.SerializeWithoutIds` for prefab JSON so persisted prefabs do not include runtime IDs.
- The portable authoring UI is `Godot/deckbuilder/scenes/entity_authoring_scene.tscn`.

## Architecture Guidelines

- Keep entity/component behavior small and focused.
- Use generated event attributes for reactive behavior, following existing patterns such as `[OnCardPlayed]`, `[OnTurnBegan]`, and `[OnRequestDamageModifiers]`.
- Add new events through the XML event definition flow only when existing events or direct narrow interfaces are insufficient.
- Avoid introducing engine dependencies into `Api`, `CardsAndPiles`, or `SummerJam1`.
- Preserve JSON compatibility where practical; if serialized component shape changes, check existing prefabs.

## Godot Guidelines

- Build Godot UI/tools in C# unless there is already a GDScript convention for the area being edited.
- Prefer code-built tool UI when matching the existing Godot screens in this project.
- Keep authoring tools portable across Linux, Windows, and macOS.
- For editor/runtime validation, use the Godot MCP tools when a session is available, then run `dotnet build Deckbuilder.csproj`.

## Working Rules

- Do not revert unrelated work in the tree.
- Keep changes scoped to the requested feature or fix.
- Update focused tests when gameplay rules, serialization behavior, prefab validity, or event flow changes.
- If a change touches prefab loading or serialization, build `Cards/Cards.sln` and the Godot project when possible.
