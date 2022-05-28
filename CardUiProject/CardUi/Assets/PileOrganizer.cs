using System.Collections.Specialized;
using UnityEngine;

public abstract class PileOrganizer : MonoBehaviour
{
    protected IPile Pile { get; set; }
    private void Awake()
    {
        Pile = GetComponentInParent<IPile>();
        Pile.Items.CollectionChanged += OnPileChanged;
    }

    protected abstract void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e);
}