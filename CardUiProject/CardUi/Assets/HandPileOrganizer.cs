using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class HandPileOrganizer : PileOrganizer
{
    [SerializeField] private float lerpRate = .01f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float maxWidth = 1f;
    [SerializeField] private float overlapRatio = .9f;
    [SerializeField] private bool rotate = true;
    [SerializeField] private float offset = 0f;


    private List<CardInHand> CardsInHand { get; } = new List<CardInHand>();

    private void Update()
    {
        float width = GetTotalWidth();
        float xPos = -width / 2f;
        int i = 0;
        foreach (CardInHand card in CardsInHand)
        {
            card.PileItem.SortHandler.SetDepth((int)Sorting.PileItem + i++);
            float halfSize = GetEffectiveCardWidth(card) / 2f;
            if (card.PileItem.IsDragging)
            {
                xPos += halfSize / 2f;
                card.PileItem.SortHandler.SetDepth((int)Sorting.DraggedPileItem);
                continue;
            }


            xPos += halfSize;
            float h = 0;
            float k = -radius;
            float theta = (Mathf.PI / 2f) + offset + (-xPos / radius);
            float x = radius * Mathf.Cos(theta) + h;
            float y = radius * Mathf.Sin(theta) + k;

            Vector3 pileItemPosition = card.PileItem.GetLocalPosition();
            Vector3 target = new Vector3(x, y, pileItemPosition.z);


            if (card.IsHovered)
            {
                var spriteRenderer = card.GetComponent<Renderer>();

                //first move it to where it would be.
                card.PileItem.SetLocalPosition(target, new Vector3());

                //then clamp it to the screen and update its transform position.
                Vector3 clampedPosition = spriteRenderer.ClampToViewport(Camera.main);
                Vector3 clampedLocalPosition =
                    transform.InverseTransformPoint(clampedPosition).WithZ(pileItemPosition.z);
                card.PileItem.SetLocalPosition(clampedLocalPosition, new Vector3());

                card.PileItem.SortHandler.SetDepth((int)Sorting.DraggedPileItem);
            }
            else
            {
                float rate = (-(lerpRate) * Time.deltaTime);
                Vector3 lerpedTarget = Vector3.Lerp(target, pileItemPosition, Mathf.Pow(rate, 2f));
                card.PileItem.SetLocalPosition(lerpedTarget, GetRotation(xPos));
            }

            xPos += halfSize;
        }
    }

    private float GetEffectiveCardWidth(CardInHand card)
    {
        return card.PileItem.GetBounds().extents.x * (card.IsHovered ? 1 : overlapRatio);
    }

    //theta = arclength/radius

    private Vector3 GetRotation(float xPos)
    {
        if (!rotate)
        {
            return Vector3.zero;
        }

        return new Vector3(0, 0, offset + (-xPos / radius));
    }

    private float GetTotalWidth()
    {
        float width = 0;

        foreach (CardInHand card in CardsInHand.Where(item => !item.PileItem.IsDragging))
        {
            width += GetEffectiveCardWidth(card);
        }

        return width;
    }

    protected override void OnPileChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (MonoBehaviour removed in e.OldItems)
            {
                CardInHand card = removed.gameObject.GetComponent<CardInHand>();
                CardsInHand.Remove(card);
                Destroy(card);
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (MonoBehaviour added in e.NewItems)
            {
                CardsInHand.Add(added.gameObject.AddComponent<CardInHand>());
                added.transform.parent = transform;
            }
        }
    }
}