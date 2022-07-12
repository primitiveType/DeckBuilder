using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Api;
using UnityEngine;

namespace App
{
    public class PileOrganizer : MonoBehaviour
    {
        protected IPileView PileView { get; set; }
        protected List<IDisposable> Disposables { get; } = new List<IDisposable>();

        protected virtual void Start()
        {
            PileView = GetComponentsInParent<IPileView>(true).First();
            PileView.Entity.Children.CollectionChanged += OnPileChanged;
            Debug.Log($"Creating {PileView.Entity.Children} children for {gameObject.name}.");
            foreach (IEntity child in PileView.Entity.Children)
            {
                OnItemAddedImmediate(child);
                OnItemAddedQueued(child);
            }
        }


        private void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity added in e.NewItems)
                {
                    OnItemAddedImmediate(added);
                    Disposables.Add(AnimationQueue.Instance.Enqueue((() => { OnItemAddedQueued(added); })));
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IEntity removed in e.OldItems)
                {
                    OnItemRemovedImmediate(removed);
                    Disposables.Add(AnimationQueue.Instance.Enqueue((() => { OnItemRemovedQueued(removed); })));
                }
            }
        }

        protected virtual void OnItemRemovedImmediate(IEntity removed)
        {
        }

        protected virtual void OnItemAddedImmediate(IEntity added)
        {
        }

        protected virtual void OnItemRemovedQueued(IEntity removed)
        {
        }

        protected virtual void OnItemAddedQueued(IEntity added)
        {
            IGameObject view = added.GetComponent<IGameObject>();
            view.gameObject.transform.SetParent(transform);
            view.gameObject.transform.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (PileView?.Entity != null)
            {
                PileView.Entity.Children.CollectionChanged -= OnPileChanged;
            }

            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
