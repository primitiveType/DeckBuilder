using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            PileView = GetComponentInParent<IPileView>();
            PileView.Entity.Children.CollectionChanged += OnPileChanged;
            foreach (IEntity child in PileView.Entity.Children)
            {
                OnItemAdded(child);
            }
        }

        private void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity added in e.NewItems)
                {
                    Disposables.Add(AnimationQueue.Instance.Enqueue((() => { OnItemAdded(added); })));
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IEntity removed in e.OldItems)
                {
                    Disposables.Add(AnimationQueue.Instance.Enqueue((() => { OnItemRemoved(removed); })));
                }
            }
        }

        protected virtual void OnItemRemoved(IEntity removed)
        {
        }

        protected virtual void OnItemAdded(IEntity added)
        {
            IGameObject view = added.GetComponent<IGameObject>();
            view.gameObject.transform.SetParent(transform);
            view.gameObject.transform.localPosition= Vector3.zero;
            
        }

        private void OnDestroy()
        {
            PileView.Entity.Children.CollectionChanged -= OnPileChanged;
            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
    }
}