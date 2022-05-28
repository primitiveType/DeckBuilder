using System;
using UnityEngine;

public class SolitaireDeckPileConstraint : MonoBehaviour, IPileConstraint
{
    public bool CanReceive(IPileItem item)
    {
        return !SolitaireHelper.Instance.GameStarted;
    }
}