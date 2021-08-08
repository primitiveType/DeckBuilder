using System.Collections.Generic;
using Data;
using MoonSharp.Interpreter;

public interface IGlobalApi
{
    void AddCard(string scriptString, string name);
    void SetCurrentBattle(Battle battle);
    int GetPlayerHealth();
    IReadOnlyList<Actor> GetEnemies();
    Battle GetCurrentBattle();
    Card CreateCardInstance(string cardName, DynValue opaqueData = null);
    Card LoadCardFromJson(string cardStr);
    Actor GetActorById(int id);
}