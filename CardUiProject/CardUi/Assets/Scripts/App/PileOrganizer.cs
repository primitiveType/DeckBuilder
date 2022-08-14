﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Api;
using CardsAndPiles;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using UnityEngine;

namespace App
{
    public class PileOrganizer : View<IPile>
    {
        [SerializeField] private Transform m_ParentTransform;
        private Transform Parent => m_ParentTransform ? m_ParentTransform : transform;
        protected IPileView PileView { get; set; }

        protected override void OnInitialized()
        {
            PileView = GetComponentsInParent<IPileView>(true).First();
            PileView.Entity.Children.CollectionChanged += OnPileChanged;
            Debug.Log($"Creating {PileView.Entity.Children} children for {gameObject.name}.");
            foreach (IEntity child in PileView.Entity.Children)
            {
                FireItemAddedImmediateWhenReady(child);
                FireItemAddedQueuedWhenReady(child);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (PileView?.Entity != null)
            {
                PileView.Entity.Children.CollectionChanged -= OnPileChanged;
            }

            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
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
            IGameObject view = await WaitForGameObject(added);
            OnItemAddedImmediate(added, view);
        }

        private async void FireItemAddedQueuedWhenReady(IEntity added)
        {
            IGameObject view = await WaitForGameObject(added);
            Disposables.Add(AnimationQueue.Instance.Enqueue(async () => { await OnItemAddedQueued(added, view); }));
        }

        private static async Task<IGameObject> WaitForGameObject(IEntity added)
        {
            IGameObject view = added.GetComponent<IGameObject>();

            if (view?.gameObject == null)
            {
                await new WaitUntil(() =>
                {
                    Debug.Log("Waiting for view to be populated.");
                    view = added.GetComponent<IGameObject>();
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

        protected virtual async Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            var scale = view.gameObject.transform.localScale;
            
            if (scale == Vector3.zero)
            {
                Debug.LogError("Scale was zero!");
            }
            view.gameObject.transform.SetParent(Parent, true);
            
            view.gameObject.transform.localPosition = Vector3.zero;
            view.gameObject.transform.localScale = Vector3.one;
        }
    }
}
