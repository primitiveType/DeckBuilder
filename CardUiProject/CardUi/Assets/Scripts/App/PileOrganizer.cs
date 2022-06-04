using System;
using System.Collections.Specialized;
using Api;
using UnityEngine;

public class PileOrganizer : MonoBehaviour
{
    protected IPileView PileView { get; set; }

    private void Start()
    {
        PileView = GetComponentInParent<IPileView>();
        PileView.Entity.Children.CollectionChanged += OnPileChanged;
        foreach (IEntity child in PileView.Entity.Children)
        {
            ParentViewToPile(child);
        }
    }

    protected virtual void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (IEntity added in e.NewItems)
            {
                ParentViewToPile(added);
            }
        }
    }

    protected virtual void ParentViewToPile(IEntity added)
    {
        IGameObject view = added.GetComponent<IGameObject>();
        view.gameObject.transform.parent = transform;
    }

    private void OnDestroy()
    {
        PileView.Entity.Children.CollectionChanged -= OnPileChanged;
    }
}