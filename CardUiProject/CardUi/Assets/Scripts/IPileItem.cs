using UnityEngine;

public interface IPileItem : IGameObject
{
    void SetLocalPosition(Vector3 transformPosition, Vector3 transformRotation);
    Vector3 GetLocalPosition();
    Bounds GetBounds();
    bool CanEnterPile(IPile pile);
    bool IsDragging { get; }

    ISortHandler SortHandler { get; }
    bool TrySendToPile(IPile pile);
}