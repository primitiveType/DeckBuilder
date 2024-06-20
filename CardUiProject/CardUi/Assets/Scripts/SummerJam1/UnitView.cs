using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Api;
using App;
using SummerJam1.Units;
using UnityEngine;

namespace SummerJam1
{
    public class UnitView : View<Unit>, IPileItemView, ISetModel //seems sus
    {
        [SerializeField] private Transform IntentRoot;
        [SerializeField] private IntentView IntentViewPrefab;
        private Vector3 RendererSize { get; set; }

        private void Awake()
        {
            List<Renderer> renderers = GetComponentsInChildren<Renderer>().ToList();
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


        protected override void OnDestroy()
        {
            base.OnDestroy();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }

        public void SetLocalPosition(Vector3 transformPosition, Vector3 transformRotation)
        {
            transform.localPosition = transformPosition;
            transform.rotation = Quaternion.Euler(transformRotation);
        }

        public void SetTargetPosition(Vector3 transformPosition, Vector3 transformRotation, bool immediate = false)
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
            return new Bounds(transform.position, RendererSize);
        }

        public bool IsDragging => false;
        public ISortHandler SortHandler { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
            UpdateCurrentIntent();
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCurrentIntent();
        }

        private void UpdateCurrentIntent()
        {
            BattleContainer battleContainer = Entity.Context.Root.GetComponent<Game>().Battle;
            if (battleContainer == null)
            {
                return;
            }

            foreach (Transform child in IntentRoot)
            {
                Destroy(child.gameObject);
            }

            Intent nextIntent = Entity.GetComponentInChildren<Intent>();

            if (nextIntent != null)
            {
                IntentView view = Instantiate(IntentViewPrefab, IntentRoot);
                view.SetModel(nextIntent);
            }
            else
            {
                Logging.LogError("Entity had null intent?");
            }
        }
    }
}
