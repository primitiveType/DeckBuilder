using System;
using System.Collections.Specialized;
using Api;
using App;
using UnityEngine;

public class PileOrganizer : MonoBehaviour
{
    protected IPileView PileView { get; set; }

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
                AnimationQueue.Instance.Enqueue(() =>
                {
                    OnItemAdded(added);
                    return null;
                });
            }
        }

        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (IEntity removed in e.OldItems)
            {
                AnimationQueue.Instance.Enqueue(() =>
                {
                    OnItemRemoved(removed);
                    return null;
                });
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
    }

    private void OnDestroy()
    {
        PileView.Entity.Children.CollectionChanged -= OnPileChanged;
    }
}