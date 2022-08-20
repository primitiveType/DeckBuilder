using System;
using System.Collections.Generic;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class VisibleDuringPlayerTurn : MonoBehaviour
    {
        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private void Start()
        {
            Disposables.Add(GameContext.Instance.Events.SubscribeToTurnBegan(OnTurnBegan));
            Disposables.Add(GameContext.Instance.Events.SubscribeToTurnEnded(OnTurnEnded));
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs item)
        {
            gameObject.SetActive(false);
        }

        private void OnTurnBegan(object sender, TurnBeganEventArgs item)
        {
            gameObject.SetActive(true);
        }
    }
}
