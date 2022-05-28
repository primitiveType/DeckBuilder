using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class PileItem : MonoBehaviour, IEndDragHandler, IPileItem, IDragHandler, IGameObject, IBeginDragHandler
{
    private Pile TargetPile { get; set; }
    private IPile CurrentPile { get; set; }
    
    private Renderer Renderer { get; set; }


    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collided with {other.gameObject.name}!");
        TargetPile = other.gameObject.GetComponent<Pile>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        if (TargetPile == null)
        {
            return;
        }

        if (TargetPile.ReceiveItem(this))
        {
            transform.SetParent(TargetPile.transform);
            CurrentPile?.RemoveItem(this);
            CurrentPile = TargetPile;
        }
    }

    public void SetLocalPosition(Vector3 transformPosition)
    {
        transform.localPosition = transformPosition;
    }
    public Vector3 GetLocalPosition()
    {
        return transform.localPosition;
    }

    public Bounds GetBounds()
    {
        return Renderer.bounds;
    }

    public bool CanEnterPile(IPile pile)
    {
        return true;
    }

    public bool IsDragging { get; private set; }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = eventData.pressEventCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] results = Physics.RaycastAll(ray, 10000, ~0, QueryTriggerInteraction.Collide);
        foreach(var result in results)
        {
            Pile pile = result.transform.GetComponent<Pile>();
            if (pile != null)
            {
                TargetPile = pile;
                Debug.Log("Found target pile!");
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
    }
}