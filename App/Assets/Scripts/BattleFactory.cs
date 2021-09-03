using System;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;

public class BattleFactory : MonoBehaviour, IBattleFactory
{
    [SerializeField] private ThreeColumnBattleProxy ThreeColumnBattleProxy;
    [SerializeField] private SquareWithCenterPointGraphProxy SquareWithCenterPointGraphProxy;

    public BattleProxy GetBattleGO(IBattle battle)
    {
        switch (battle.Graph)
        {
            case SquareWithCenterPointGraph _:
                return Instantiate(SquareWithCenterPointGraphProxy);
            case ThreeColumnGraph _:
                return Instantiate(ThreeColumnBattleProxy);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

  
}