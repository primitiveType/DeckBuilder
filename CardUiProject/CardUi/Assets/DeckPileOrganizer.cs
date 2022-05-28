using System.Collections.Specialized;
using UnityEngine;

public class DeckPileOrganizer : PileOrganizer
{
    [SerializeField] private Transform parentTransform;

    protected override void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (MonoBehaviour added in e.NewItems)
            {
                added.transform.SetParent(parentTransform);
            }
        }
    }
}