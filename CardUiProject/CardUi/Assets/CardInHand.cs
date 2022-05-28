using UnityEngine;
using UnityEngine.EventSystems;

public class CardInHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private HandPileOrganizer Organizer { get; set; }
    private PileItem PileItem { get; set; }

    private void Awake()
    {
        Debug.Log("Card is in hand!");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // transform.localScale = Vector3.one * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // transform.localScale = Vector3.one;
    }
}