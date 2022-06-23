using System.Collections.Specialized;
using SummerJam1;
using UnityEngine;

public class HideGameObjectIfNoPrizes : MonoBehaviour
{
    private void Awake()
    {
        SummerJam1Context.Instance.Game.PrizePile.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        SummerJam1Context.Instance.Game.RelicPrizePile.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        NewMethod();
    }

    private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        NewMethod();
    }

    private void NewMethod()
    {
        gameObject.SetActive(SummerJam1Context.Instance.Game.PrizePile.Entity.Children.Count == 0 &&
                             SummerJam1Context.Instance.Game.RelicPrizePile.Entity.Children.Count == 0
        );
    }
}
