using System.Collections.Generic;
using Data;
using MoonSharp.Interpreter;

public interface IContext
{
    void SetCurrentBattle(Battle battle);
    int GetPlayerHealth();
    IReadOnlyList<Actor> GetEnemies();
    Battle GetCurrentBattle();

    Card CreateCardInstance(string cardName);

    Actor GetActorById(int id);
    IGameEventHandler Events { get; }
    void AddEntity(GameEntity entity);
    Script GetCardScript(string name);
    int GetNextEntityId();
}