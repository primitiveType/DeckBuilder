using System.Collections.Specialized;
using UnityEngine;

public class PileOrganizer : MonoBehaviour
{
    protected IPile Pile { get; set; }
    private void Awake()
    {
        Pile = GetComponentInParent<IPile>();
        Pile.Items.CollectionChanged += OnPileChanged;
    }

    protected virtual void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (MonoBehaviour added in e.NewItems)
            {
                added.transform.parent = transform;
            }
        }
    }
}