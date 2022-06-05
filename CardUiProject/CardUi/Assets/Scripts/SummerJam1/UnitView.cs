using System;
using Common;
using UnityEngine;

namespace SummerJam1
{
    public class UnitView : View<Unit>, IPileItemView //seems sus
    {
        private Renderer Renderer { get; set; }

        private void Awake()
        {
            Renderer = GetComponentInChildren<Renderer>();

            SortHandler = GetComponent<ISortHandler>();
            SortHandler.SetDepth((int)Sorting.PileItem);
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

        public bool IsDragging => false;
        public ISortHandler SortHandler { get; set; }
    }
}