using System;
using System.Collections.Specialized;
using SummerJam1;
using UnityEngine;

public class HideGameObjectIfNoPrizes : MonoBehaviour
{
    private void Awake()
    {
        SummerJam1Context.Instance.Game.PrizePile.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        SummerJam1Context.Instance.Game.RelicPrizePile.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        UpdateVisibility();
    }

    private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        gameObject.SetActive(SummerJam1Context.Instance.Game.PrizePile.Entity.Children.Count > 0);
    }

    private void OnDestroy()
    {
        SummerJam1Context.Instance.Game.PrizePile.Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        SummerJam1Context.Instance.Game.RelicPrizePile.Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
    }
}
