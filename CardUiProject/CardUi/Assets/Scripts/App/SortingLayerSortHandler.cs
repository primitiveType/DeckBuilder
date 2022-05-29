using UnityEngine;

public class SortingLayerSortHandler : MonoBehaviour, ISortHandler
{
    [SerializeField] private SpriteRenderer SpriteRenderer;

    public void SetDepth(int depth)
    {
        SpriteRenderer.sortingOrder = depth;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -depth * .01f);
    }
}