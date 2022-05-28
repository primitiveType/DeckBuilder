﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;


public class HandPileOrganizer : PileOrganizer
{
    [SerializeField] private float lerpRate = .01f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float maxWidth = 1f;
    [SerializeField] private float overlapRatio = .9f;
    [SerializeField] private bool rotate = true;
    [SerializeField] private float offset = 0f;
    [SerializeField] private float hoveredOverlapRotation = 1f;


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
            float x;
            float y;
            if (rotate)
            {
                float h = 0;
                float k = -radius;
                float theta = (Mathf.PI / 2f) + offset + (-xPos / radius);
                x = radius * Mathf.Cos(theta) + h;
                y = radius * Mathf.Sin(theta) + k;
            }
            else
            {
                x = xPos;
                y = 0;
            }

            Vector3 pileItemPosition = card.PileItem.GetLocalPosition();
            Vector3 target = new Vector3(x, y, pileItemPosition.z);


            if (card.IsHovered)
            {
                SetHoveredPosition(card, target, pileItemPosition);
            }
            else
            {
                float rate = (-(lerpRate) * Time.deltaTime);
                Vector3 lerpedTarget = VectorExtensions.Damp( pileItemPosition, target, lerpRate, Time.deltaTime);
                card.PileItem.SetLocalPosition(lerpedTarget, GetRotation(xPos));
            }

            xPos += halfSize;
        }
    }

    private void SetHoveredPosition(CardInHand card, Vector3 target, Vector3 pileItemPosition)
    {
        //first move it to where it would be.
        card.PileItem.SetLocalPosition(target, new Vector3());

        //then clamp it to the screen and update its transform position.
        var spriteRenderer = card.GetComponent<Renderer>();
        Vector3 clampedPosition = spriteRenderer.ClampToViewport(Camera.main);
        Vector3 clampedLocalPosition =
            transform.InverseTransformPoint(clampedPosition).WithZ(pileItemPosition.z);
        card.PileItem.SetLocalPosition(clampedLocalPosition, new Vector3());

        card.PileItem.SortHandler.SetDepth((int)Sorting.DraggedPileItem);
    }

    private float GetEffectiveCardWidth(CardInHand card)
    {
        return 2 * card.PileItem.GetBounds().extents.x * (card.IsHovered ? hoveredOverlapRotation : overlapRatio);
    }

    //theta = arclength/radius

    private Vector3 GetRotation(float xPos)
    {
        if (!rotate)
        {
            return Vector3.zero;
        }

        return new Vector3(0, 0, Mathf.Rad2Deg * (offset + (-xPos / radius)));
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