using System;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;

public class BattleFactory : MonoBehaviour, IBattleFactory
{
    [SerializeField] private HexBattleProxy BattleProxy;

    public BattleProxy GetBattleGO(IBattle battle)
    {
        return Instantiate(BattleProxy);
    }
}