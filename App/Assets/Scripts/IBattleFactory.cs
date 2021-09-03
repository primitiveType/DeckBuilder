using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;

public interface IBattleFactory
{
    BattleProxy GetBattleGO(IBattle battle);
}