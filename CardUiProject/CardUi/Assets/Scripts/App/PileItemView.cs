using System.Collections;
using System.Linq;
using App.Utility;
using CardsAndPiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App
{
    [RequireComponent(typeof(ISortHandler))]
    public class PileItemView<T> : View<T>, IEndDragHandler, IPileItemView, IDragHandler, IGameObject,
        IBeginDragHandler where T : IPileItem
    {
        private PileView TargetPileView { get; set; }

        public bool IsInLayoutGroup; //feels a bit hacky, but hopefully reliable?
        
        public ISortHandler SortHandler { get; private set; }

        private float lerpRate = 13;

        private void Awake()
        {
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
            
        }

        protected override void Start()
        {
            base.Start();
            IsInLayoutGroup = GetComponentInParent<LayoutGroup>() != null;
        }

        private Vector3 BoundsSize { get; set; }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log($"collided with {other.gameObject.name}!");
            TargetPileView = other.gameObject.GetComponentInParent<PileView>();
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

            if (!TrySendToPile(TargetPileView))
            {
                Debug.Log($"Failed to add {name} to {TargetPileView.name}.");
            }
        }

        public virtual bool TrySendToPile(IPileView pileView)
        {
            bool success = Entity.TrySetParent(pileView.Entity);

            IsInLayoutGroup = GetComponentInParent<LayoutGroup>() != null;

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
            if (!IsDragging && !IsInLayoutGroup)
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


        public bool IsDragging { get; private set; }

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
            IsDragging = true;
            transform.localRotation = Quaternion.identity;
        }
    }
}
