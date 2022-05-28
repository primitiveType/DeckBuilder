using System;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class HandPileOrganizer : PileOrganizer
{
    [SerializeField] private float lerpRate = .01f;

    private void Update()
    {
        float width = 0;
        foreach (IPileItem pileItem in Pile.Items.Where(item => !item.IsDragging))
        {
            width += pileItem.GetBounds().size.x;
        }

        float xPos = -width/2f;
        foreach (IPileItem pileItem in Pile.Items.Where(item => !item.IsDragging))
        {
            float halfSize = pileItem.GetBounds().extents.x;
            xPos += halfSize;
            Vector3 target = new Vector3(xPos, 0, 0);
            float rate = (-(lerpRate) * Time.deltaTime);
            Vector3 lerpedTarget = Vector3.Lerp(target, pileItem.GetLocalPosition(), Mathf.Pow(rate, 2f));
            pileItem.SetLocalPosition(lerpedTarget);
            xPos += halfSize;
        }
    }

    protected override void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (MonoBehaviour removed in e.OldItems)
            {
                Destroy(removed.gameObject.GetComponent<CardInHand>());
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (MonoBehaviour added in e.NewItems)
            {
                added.gameObject.AddComponent<CardInHand>();
            }
        }
    }
}