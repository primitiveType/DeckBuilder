using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 targetPosition { get; set; }

    private Vector3 offset { get; set; }
    private bool dragging { get; set; }

    // [SerializeField] private Camera m_EventCamera;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {//should lerp
            transform.position = targetPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var cursorWorld =
            eventData.pressEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
                transform.position.z));
        targetPosition = new Vector3(offset.x + cursorWorld.x, offset.y + cursorWorld.y, transform.position.z);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        Debug.Log("Begin drag!");
        var cursorWorld =
            eventData.pressEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
                transform.position.z));

        offset = transform.position - cursorWorld;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        Debug.Log("End drag!");
    }
}