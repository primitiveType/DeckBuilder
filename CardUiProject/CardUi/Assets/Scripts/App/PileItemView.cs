using System.Collections.Generic;
using System.Linq;
using Api;
using App.Utility;
using CardsAndPiles;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App
{
    
    [RequireComponent(typeof(ISortHandler))]
    public class PileItemView<T> : View<T>, IEndDragHandler, IPileItemView, IDragHandler, IGameObject,
        IBeginDragHandler
    {
        public bool IsInLayoutGroup; //feels a bit hacky, but hopefully reliable?

        private readonly float lerpRate = 8;
        private IEntity TargetDrag { get; set; }

        private Vector3 BoundsSize { get; set; } 

        //during turn, setting a new target should interrupt.
        //during combat, setting a new target should wait until old lerp is finished.

        private Vector3 TargetPosition { get; set; }
        private Vector3 TargetRotation { get; set; }

        public IPile CurrentPile { get; set; }

        private void Awake()
        {
            SortHandler = GetComponent<ISortHandler>();
            SortHandler.SetDepth((int)Sorting.PileItem);
            BoxCollider colliders = GetComponentInChildren<BoxCollider>(true);
            BoundsSize = colliders.size;
        }

        protected override void Start()
        {
            base.Start();
            IsInLayoutGroup = GetComponentInParent<LayoutGroup>() != null;
        }

        private void Update()
        {
            IsInLayoutGroup = (transform.parent != null ? transform.parent.GetComponentInParent<LayoutGroup>() : null) != null;

            if (!IsDragging && !IsInLayoutGroup)
            {
                Interpolate(TargetPosition, TargetRotation);
            }
            else if (IsInLayoutGroup)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            TargetDrag = other.gameObject.GetComponentInParent<IView>().Entity;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            transform.localRotation = Quaternion.identity;
            CurrentPile = transform.GetComponentInParent<IPileView>().Entity.GetComponent<IPile>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Ray ray = eventData.pressEventCamera.ScreenPointToRay(Input.mousePosition);
            //
            // RaycastHit[] results = Physics.RaycastAll(ray, 10000, ~0, QueryTriggerInteraction.Collide);
            //
            IEntity target = null;
            var results = eventData.hovered;
            foreach (var result in results)
            {
                IView pileView = result.transform.GetComponentInParent<IView>();
                if (pileView != null && pileView.Entity.GetComponent<IPile>() != CurrentPile)
                {
                    target = pileView.Entity;
                    // Logging.Log("Found target pile view : " + pileView.name);
                }
            }

            TargetDrag = target;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;

            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.PlayCard))
            {
                return;
            }

            if (TargetDrag == null || TargetDrag == GetComponentInParent<PileView>())
            {
                return;
            }

            if (!TrySendToPile(TargetDrag))
            {
                Logging.Log($"Failed to add {name} to {TargetDrag.GetName()}.");
            }
            else
            {
                Logging.Log($"Adding {name} to {TargetDrag.GetName()}.");

                //We should maybe instead just navigate the model hierarchy...
                // CurrentPile = TargetDrag.GetComponentInParent<PileView>().Model;
            }
        }

        public ISortHandler SortHandler { get; private set; }

        public void SetTargetPosition(Vector3 transformPosition, Vector3 transformRotation, bool immediate = false)
        {
            if (immediate)
            {
                TargetPosition = transformPosition;
                TargetRotation = transformRotation;
            }
            else
            {
                Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
                {
                    TargetPosition = transformPosition;
                    TargetRotation = transformRotation;
                }));
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
            Transform transform1 = transform;
            return new Bounds(transform1.position, BoundsSize * transform1.lossyScale.x);
        }


        public bool IsDragging { get; private set; }

        public virtual bool TrySendToPile(IEntity target)
        {
            bool success = Entity.TrySetParent(target);


            return success;
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
    }
}
