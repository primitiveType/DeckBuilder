using System;
using System.Collections.Generic;
using App;
using CardsAndPiles;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using UnityEngine;

namespace SummerJam1
{
    public class DamageAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject Prefab;
        private SummerJam1Events Events => SummerJam1Context.Instance.Events;
        private Game Game => SummerJam1Context.Instance.Game;
        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private void Awake()
        {
            Disposables.Add(Events.SubscribeToDamageDealt(OnDamageDealt));
        }

        private void OnDamageDealt(object sender, DamageDealtEventArgs item)
        {
            var targetGO = item.EntityId.GetComponent<IGameObject>();
            AnimationQueue.Instance.Enqueue(async () =>
                {
                    GameObject instance;
                    if (targetGO?.gameObject == null)
                    {
                        return;
                    }

                    instance = Instantiate(Prefab);
                    instance.transform.position = targetGO.gameObject.transform.position;

                    await new WaitForSeconds(.5f);
                    Destroy(instance);
                }
            );
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

