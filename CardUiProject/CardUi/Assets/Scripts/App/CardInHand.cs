using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using Stateless;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App
{
    public class CardInHand : View<IPileItem>, IPointerEnterHandler, IPointerExitHandler, IPileItemView, IGameObject, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        private bool IsHovered { get; set; }
        public IPileItemView PileItemView { get; private set; }

        public bool DisplayWholeCard => IsHovered && enabled && WarmedUp;
        private bool WarmedUp { get; set; }
        private PileView TargetPileView { get; set; }

        private DraggableComponent DraggableComponent { get; set; }

        public ISortHandler SortHandler { get; private set; }

        private float lerpRate = 13;


        private Vector3 BoundsSize { get; set; }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"collided with {other.gameObject.name}!");
            TargetPileView = other.gameObject.GetComponentInParent<PileView>();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            

            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.PlayCard))
            {
                return;
            }

            if (TargetPileView == null || TargetPileView == GetComponentInParent<PileView>())
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
            bool success = Entity.TrySetParent(pileView.Entity);

            return success;
        }

        //during turn, setting a new target should interrupt.
        //during combat, setting a new target should wait until old lerp is finished.

        private Vector3 TargetPosition { get; set; }
        private Vector3 TargetRotation { get; set; }

        public void SetTargetPosition(Vector3 transformPosition, Vector3 transformRotation, bool immediate = false)
        {
            if (immediate)
            {
                TargetPosition = transformPosition;
                TargetRotation = transformRotation;
            }
            else
            {
                Disposables.Add(AnimationQueue.Instance.Enqueue((() =>
                {
                    TargetPosition = transformPosition;
                    TargetRotation = transformRotation;
                })));
            }
        }

        private void Update()
        {
            if (!IsDragging)
            {
                Interpolate(TargetPosition, TargetRotation);
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
            var transform1 = transform;
            return new Bounds(transform1.position, BoundsSize * transform1.lossyScale.x);
        }


        public bool IsDragging => DraggableComponent.Dragging;

        public void OnDrag(PointerEventData eventData)
        {
            Ray ray = eventData.pressEventCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] results = Physics.RaycastAll(ray, 10000, ~0, QueryTriggerInteraction.Collide);
            foreach (RaycastHit result in results)
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
            transform.localRotation = Quaternion.identity;
        }

        private void Awake()
        {
            DraggableComponent = GetComponent<DraggableComponent>();
            SortHandler = GetComponent<ISortHandler>();
            SortHandler.SetDepth((int)Sorting.PileItem);

            var colliders = GetComponentsInChildren<Collider>().ToList();
            if (colliders.Count > 0)
            {
                Bounds bounds = colliders[0].bounds;
                foreach (Collider renderer1 in colliders)
                {
                    bounds.Encapsulate(renderer1.bounds);
                }

                BoundsSize = bounds.size;
            }

            ResetScale();

            PileItemView = GetComponentInParent<IPileItemView>();
            if (PileItemView == null)
            {
                Debug.LogError("Pile item not found when adding card to hand!");
            }

            StartCoroutine(WarmUp());
            InputStateManager.Instance.StateMachine.OnTransitioned(OnInputStateChanged);
            SetEnabledStateFromMachine();
        }

        private IEnumerator WarmUp()
        {
            yield return new WaitForSeconds(.5f);
            WarmedUp = true;
        }


        private void OnInputStateChanged(StateMachine<InputState, InputAction>.Transition obj)
        {
            if (this == null)
            {
                return; //hack for now. Can't unsubscribe from state machine.
            }

            SetEnabledStateFromMachine();
        }

        private void SetEnabledStateFromMachine()
        {
            enabled = IsDragging || InputStateManager.Instance.StateMachine.CanFire(InputAction.Hover);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsDragging)
                return;
            IsHovered = true;
            InputStateManager.Instance.StateMachine.Fire(InputAction.Hover);
            transform.localScale = Vector3.one * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsDragging)
                return;
            IsHovered = false;
            InputStateManager.Instance.StateMachine.Fire(InputAction.EndHover);

            ResetScale();
        }

        private void OnDisable()
        {
            ResetScale();
            IsHovered = false; //this feels better but its a little odd.
        }

        private void ResetScale()
        {
            transform.localScale = Vector3.one;
        }
    }
}
