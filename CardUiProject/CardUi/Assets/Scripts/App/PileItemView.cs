using System;
using System.Collections;
using System.Linq;
using App;
using CardsAndPiles;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ISortHandler))]
public class PileItemView<T> : View<T>, IEndDragHandler, IPileItemView, IDragHandler, IGameObject,
    IBeginDragHandler where T : IPileItem
{
    private PileView TargetPileView { get; set; }


    public ISortHandler SortHandler { get; private set; }

    private float lerpRate = 13;

    private void Awake()
    {
        SortHandler = GetComponent<ISortHandler>();
        SortHandler.SetDepth((int)Sorting.PileItem);

        var renderers = GetComponentsInChildren<Renderer>().ToList();
        if (renderers.Count > 0)
        {
            Bounds bounds = renderers[0].bounds;
            foreach (Renderer renderer1 in renderers)
            {
                bounds.Encapsulate(renderer1.bounds);
            }

            RendererSize = bounds.size;
        }
    }

    public Vector3 RendererSize;

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

    public virtual bool TrySendToPile(IPileView pileView)
    {
        return Entity.TrySetParent(pileView.Entity);
    }

    //during turn, setting a new target should interrupt.
    //during combat, setting a new target should wait until old lerp is finished.

    private Vector3 TargetPosition;
    private Vector3 TargetRotation;

    public void SetTargetPosition(Vector3 transformPosition, Vector3 transformRotation, bool immediate = false)
    {
        if (immediate)
        {
            TargetPosition = transformPosition;
            TargetRotation = transformRotation;
        }
        else
        {
            AnimationQueue.Instance.Enqueue(() =>
            {
                TargetPosition = transformPosition;
                TargetRotation = transformRotation;
                return null;
            });
        }
    }

    private void Update()
    {
        if (!IsDragging)
        {
            Interpolate(TargetPosition, TargetRotation);
        }
    }

    private IEnumerator LerpToTarget(Vector3 transformPosition, Vector3 transformRotation)
    {
        while (Vector3.Distance(transform.localPosition, transformPosition) > Vector3.kEpsilon &&
               Vector3.Angle(transform.localRotation.eulerAngles, transformRotation) > Vector3.kEpsilon)
        {
            Interpolate(transformPosition, transformRotation);
            yield return null;
        }
    }

    private void Interpolate(Vector3 transformPosition, Vector3 transformRotation)
    {
        Vector3 lerpedTarget =
            VectorExtensions.Damp(transform.localPosition, transformPosition, lerpRate, Time.deltaTime);
        transform.localPosition = lerpedTarget;

        Vector3 lerpedRotation = transformRotation;
        //     VectorExtensions.Damp(transform.rotation.eulerAngles, transformRotation, lerpRate, Time.deltaTime);

        transform.localRotation = Quaternion.Euler(lerpedRotation);
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
        return new Bounds(transform.position, RendererSize * transform.lossyScale.x);
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
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        transform.localRotation = Quaternion.identity;
    }
}