using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private HandPileOrganizer Organizer { get; set; }
    public PileItem PileItem { get; private set; }
    public bool IsHovered { get; private set; }

    private void Awake()
    {
        Debug.Log("Card is in hand!");
        PileItem = GetComponentInParent<PileItem>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovered = true;
        transform.localScale = Vector3.one * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
        ResetScale();
    }

    private void OnDestroy()
    {
        ResetScale();
    }

    private void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
}