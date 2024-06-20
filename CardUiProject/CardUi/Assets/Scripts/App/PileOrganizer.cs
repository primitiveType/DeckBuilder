using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Api;
using CardsAndPiles;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using UnityEngine;

namespace App
{
    public class PileOrganizer : View<IComponent>
    {
        [SerializeField] private Transform m_ParentTransform;

        protected virtual bool RequireChildView => true;

        protected Transform Parent => m_ParentTransform ? m_ParentTransform : transform;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Entity != null)
            {
                Entity.Children.CollectionChanged -= OnPileChanged;
            }

            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
        // protected IPileView PileView { get; set; }

        protected override void OnInitialized()
        {
            // PileView = GetComponentsInParent<IPileView>(true).First();
            Entity.Children.CollectionChanged += OnPileChanged;
            foreach (IEntity child in Entity.Children)
            {
                FireItemAddedImmediateWhenReady(child);
                FireItemAddedQueuedWhenReady(child);
            }
        }


        private void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity added in e.NewItems)
                {
                    FireItemAddedImmediateWhenReady(added);
                    FireItemAddedQueuedWhenReady(added);
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IEntity removed in e.OldItems)
                {
                    OnItemRemovedImmediate(removed);
                    Disposables.Add(AnimationQueue.Instance.Enqueue(() => { OnItemRemovedQueued(removed); }));
                }
            }
        }

        private async void FireItemAddedImmediateWhenReady(IEntity added)
        {
            IGameObject view = null;
            if (RequireChildView)
            {
                view = await WaitForGameObject(added);
            }

            OnItemAddedImmediate(added, view);
        }

        private async void FireItemAddedQueuedWhenReady(IEntity added)
        {
            IGameObject view = null;

            if (RequireChildView)
            {
                view = await WaitForGameObject(added);
            }

            Disposables.Add(AnimationQueue.Instance.Enqueue(async () => { await OnItemAddedQueued(added, view); }));
        }

        private async Task<IGameObject> WaitForGameObject(IEntity added)
        {
            IGameObject view = added.GetComponent<IGameObject>();

            if (view?.gameObject == null)
            {
                int tries = 0;
                await new WaitUntil(() =>
                {
                    // Debug.Log($"Waiting for view to be populated {tries}.");
                    view = added.GetComponent<IGameObject>();
                    if (tries++ > 2)//should never take two frames for this to happen!
                    {
                        Debug.Log($"{name} Gave up Waiting for view to be populated. {added.GetDebugString()}");
                        return true;
                    }

                    return view?.gameObject != null;
                });
            }

            return view;
        }

        protected virtual void OnItemRemovedImmediate(IEntity removed)
        {
        }

        protected virtual void OnItemAddedImmediate(IEntity added, IGameObject view)
        {
        }

        protected virtual void OnItemRemovedQueued(IEntity removed)
        {
        }

        protected virtual Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            if (view == null)
            {
                return Task.CompletedTask;
            }
            Vector3 scale = view.gameObject.transform.localScale;

            if (scale == Vector3.zero)
            {
                Debug.LogError("Scale was zero!");
            }

            view.gameObject.transform.SetParent(Parent, true);

            // view.gameObject.transform.localPosition = Vector3.zero;
            view.gameObject.transform.localScale = Vector3.one;
            view.gameObject.transform.rotation = Quaternion.identity;
            return Task.CompletedTask;
        }
    }
}
