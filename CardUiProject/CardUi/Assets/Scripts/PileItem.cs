using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(ISortHandler))]
public class PileItem : MonoBehaviour, IEndDragHandler, IPileItem, IDragHandler, IGameObject, IBeginDragHandler
{
    private Pile TargetPile { get; set; }
    private IPile CurrentPile { get; set; }

    private Renderer Renderer { get; set; }

    public ISortHandler SortHandler { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
        SortHandler = GetComponent<ISortHandler>();
        SortHandler.SetDepth((int)Sorting.PileItem);
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
            CurrentPile?.RemoveItem(this);
            CurrentPile = TargetPile;
        }
    }


    public void SetLocalPosition(Vector3 transformPosition, Vector3 transformRotation)
    {
        transform.localPosition = transformPosition;
        transform.rotation = Quaternion.Euler(transformRotation);
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
        foreach (var result in results)
        {
            Pile pile = result.transform.GetComponent<Pile>();
            if (pile != null)
            {
                TargetPile = pile;
                Debug.Log($"Found target pile {pile.name}!");
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        transform.localRotation = Quaternion.identity;
    }
}

public interface ISortHandler
{
    void SetDepth(int depth);
}