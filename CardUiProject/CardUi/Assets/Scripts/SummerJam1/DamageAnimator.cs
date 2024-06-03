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
        private TextPopupManager m_TextDamageManager => TextPopupManager.Instance;

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
                QueueText(targetGo.gameObject.transform, item.Amount.ToString(), TextPopupManager.HEALING_TEXT);
            }
        }

        private void OnDamageDealt(object sender, DamageDealtEventArgs item)
        {
            var targetGo = item.EntityId.GetComponent<IGameObject>();
            if (targetGo != null)
            {
                QueueText(targetGo.gameObject.transform, item.Amount.ToString(), TextPopupManager.DAMAGE_TEXT);
            }
        }

        private void QueueText(Transform target, string text, string key)
        {
            AnimationQueue.Instance.Enqueue(async () =>
            {
                await new WaitForEndOfFrame();
                m_TextDamageManager.Add(text, target.gameObject, key);
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
