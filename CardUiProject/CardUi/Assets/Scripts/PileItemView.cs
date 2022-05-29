using System;
using CardsAndPiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(ISortHandler))]
public class PileItemView : View<IPileItem>, IEndDragHandler, IPileItemView, IDragHandler, IGameObject,
    IBeginDragHandler
{
    private PileView TargetPileView { get; set; }
    private IPileView CurrentPileView { get; set; }

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
        TargetPileView = other.gameObject.GetComponent<PileView>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        if (TargetPileView == null)
        {
            return;
        }

        if (!TrySendToPile(TargetPileView))
        {
            Debug.Log($"Failed to add {name} to {TargetPileView.name}.");
        }
    }

    public bool TrySendToPile(IPileView pileView)
    {
        return pileView.Model.ReceiveItem(this.Model);
        // if (pileView.ReceiveItem(this))
        // {
        //     CurrentPileView?.RemoveItem(this);
        //     CurrentPileView = pileView;
        //     return true;
        // }
        //
        //
        // Debug.Log($"Failed to add {name} to {pileView}");
        // return false;
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

    public bool CanEnterPile(IPileView pileView)
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
            PileView pileView = result.transform.GetComponent<PileView>();
            if (pileView != null)
            {
                TargetPileView = pileView;
                Debug.Log($"Found target pile {pileView.name}!");
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        transform.localRotation = Quaternion.identity;
    }
}