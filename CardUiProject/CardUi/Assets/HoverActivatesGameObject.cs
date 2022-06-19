using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverActivatesGameObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject ToHide;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToHide.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToHide.SetActive(false);
    }
}
