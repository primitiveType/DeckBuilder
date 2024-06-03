using System;
using System.Collections.Generic;
using UnityEngine;

namespace SummerJam1
{
    public class VisibleDuringDungeonPhaseTurn : MonoBehaviour
    {
        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private void Start()
        {
            Disposables.Add(GameContext.Instance.Events.SubscribeToDungeonPhaseStarted(OnTurnBegan));
            Disposables.Add(GameContext.Instance.Events.SubscribeToDungeonPhaseEnded(OnTurnEnded));
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }

        private void OnTurnEnded(object sender, DungeonPhaseEndedEventArgs dungeonPhaseEndedEventArgs)
        {
            gameObject.SetActive(false);
        }

        private void OnTurnBegan(object sender, DungeonPhaseStartedEventArgs dungeonPhaseStartedEventArgs)
        {
            gameObject.SetActive(true);
        }
    }
}