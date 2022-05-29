using UnityEngine;

public interface IPileItemView
{
    void SetLocalPosition(Vector3 transformPosition, Vector3 transformRotation);
    Vector3 GetLocalPosition();
    Bounds GetBounds();
    bool IsDragging { get; }

    ISortHandler SortHandler { get; }
    
  

}