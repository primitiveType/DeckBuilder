using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace SummerJam1
{
    public class ShowWhenRemovingCard : MonoBehaviour
    {
        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private void Awake()
        {
            Disposables.Add(SummerJam1Context.Instance.Events.SubscribeToRequestRemoveCard(OnCardRemoval));
            gameObject.SetActive(false);
        }

        private void OnCardRemoval(object sender, RequestRemoveCardEventArgs item)
        {
            gameObject.SetActive(true);         
            SummerJam1Context.Instance.Game.Deck.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            gameObject.SetActive(false);
            SummerJam1Context.Instance.Game.Deck.Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }

        private void OnDestroy()
        {
            SummerJam1Context.Instance.Game.Deck.Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
    }
}