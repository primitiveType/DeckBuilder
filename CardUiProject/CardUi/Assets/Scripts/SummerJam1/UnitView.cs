using System;
using System.Linq;
using Common;
using SummerJam1.Units;
using UnityEngine;

namespace SummerJam1
{
    public class UnitView : View<Unit>, IPileItemView, ISetModel //seems sus
    {
        private Vector3 RendererSize { get; set; }

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
    }
}