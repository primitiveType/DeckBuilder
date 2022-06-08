using UnityEngine;

public interface IPileItemView
{
    void SetTargetPosition(Vector3 transformPosition, Vector3 transformRotation, bool immediate = false);
    Vector3 GetLocalPosition();
    Bounds GetBounds();
    bool IsDragging { get; }

    ISortHandler SortHandler { get; }


    void SetLocalPosition(Vector3 transformPosition, Vector3 transformRotation);
}