using System;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class RelicView : View<RelicComponent>, ISetModel, IPileItemView
    {
        public void SetTargetPosition(Vector3 transformPosition, bool immediate = false)
        {
            transform.localPosition = new Vector3();

        }

        public void SetTargetRotation(Vector3 transformRotation, bool immediate = false)
        {
            transform.localRotation = Quaternion.Euler(transformRotation);
        }

        public Vector3 GetLocalPosition()
        {
            return new Vector3();
        }

        public Bounds GetBounds()
        {
            return new Bounds();
        }

        public bool IsDragging { get; } = false;
        public ISortHandler SortHandler { get; private set; }

        public void SetLocalPosition(Vector3 transformPosition)
        {
            transform.localPosition = new Vector3();
        }
    }
}
