using System;
using System.Collections.Specialized;
using SummerJam1;
using UnityEngine;

public class HideGameObjectIfNoPrizes : MonoBehaviour
{
    private void Start()
    {
        GameContext.Instance.Game.PrizePile.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        UpdateVisibility();
    }

    private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        gameObject.SetActive(GameContext.Instance.Game.PrizePile.Entity.Children.Count > 0);
    }

    private void OnDestroy()
    {
        GameContext.Instance.Game.PrizePile.Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
    }
}
