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
        private PileView TargetPileView { get; set; }

        private Vector3 BoundsSize { get; set; }

        //during turn, setting a new target should interrupt.
        //during combat, setting a new target should wait until old lerp is finished.

        private Vector3 TargetPosition { get; set; }
        private Vector3 TargetRotation { get; set; }

        public IPileView CurrentPileView { get; set; }

        private void Awake()
        {
            SortHandler = GetComponent<ISortHandler>();
            SortHandler.SetDepth((int)Sorting.PileItem);

            List<Collider> colliders = GetComponentsInChildren<Collider>().ToList();
            if (colliders.Count > 0)
            {
                Bounds bounds = colliders[0].bounds;
                foreach (Collider renderer1 in colliders)
                {
                    bounds.Encapsulate(renderer1.bounds);
                }

                BoundsSize = bounds.size;
            }
        }

        protected override void Start()
        {
            base.Start();
            IsInLayoutGroup = GetComponentInParent<LayoutGroup>() != null;
        }

        private void Update()
        {
            IsInLayoutGroup = transform.parent?.GetComponentInParent<LayoutGroup>() != null;

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
            TargetPileView = other.gameObject.GetComponentInParent<PileView>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            transform.localRotation = Quaternion.identity;
            CurrentPileView = transform.GetComponentInParent<IPileView>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Ray ray = eventData.pressEventCamera.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit[] results = Physics.RaycastAll(ray, 10000, ~0, QueryTriggerInteraction.Collide);

            PileView target = null;
            foreach (var result in results)
            {
                PileView pileView = result.transform.GetComponentInParent<PileView>();
                if (pileView != null && pileView != CurrentPileView)
                {
                    target = pileView;
                    Logging.Log("Found target pile view : " + pileView.name);
                }
            }

            TargetPileView = target;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;

            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.PlayCard))
            {
                return;
            }

            if (TargetPileView == null || TargetPileView == GetComponentInParent<PileView>())
            {
                return;
            }

            if (!TrySendToPile(TargetPileView.Entity))
            {
                Logging.Log($"Failed to add {name} to {TargetPileView}.");
            }
            else
            {
                CurrentPileView = TargetPileView;
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

        public virtual bool TrySendToPile(IEntity pileView)
        {
            bool success = Entity.TrySetParent(pileView);


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
