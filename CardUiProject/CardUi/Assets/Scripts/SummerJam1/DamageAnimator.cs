using System;
using System.Collections.Generic;
using App;
using CardsAndPiles;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using Guirao.UltimateTextDamage;
using UnityEngine;

namespace SummerJam1
{
    public class DamageAnimator : MonoBehaviour
    {
        [SerializeField] private UltimateTextDamageManager m_TextDamageManager;

        private SummerJam1Events Events => GameContext.Instance.Events;
        private Game Game => GameContext.Instance.Game;
        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private void Awake()
        {
            Disposables.Add(Events.SubscribeToDamageDealt(OnDamageDealt));
            Disposables.Add(Events.SubscribeToHealDealt(OnHealDealt));
        }

        private void OnHealDealt(object sender, HealDealtEventArgs item)
        {
            var targetGo = item.EntityId.GetComponent<IGameObject>();
            if (targetGo != null)
            {
                QueueText(targetGo.gameObject.transform, item.Amount.ToString(), "healing");
            }
        }

        private void OnDamageDealt(object sender, DamageDealtEventArgs item)
        {
            var targetGo = item.EntityId.GetComponent<IGameObject>();
            if (targetGo != null)
            {
                QueueText(targetGo.gameObject.transform, item.Amount.ToString(), "damage");
            }
        }

        private void QueueText(Transform target, string text, string key)
        {
            AnimationQueue.Instance.Enqueue(async () =>
            {
                await new WaitForEndOfFrame();
                m_TextDamageManager.Add(text, target, key);
            });
        }

        private void OnDestroy()
        {
            for (var i = 0; i < Disposables.Count; i++)
            {
                Disposables[i].Dispose();
            }
        }
    }
}
