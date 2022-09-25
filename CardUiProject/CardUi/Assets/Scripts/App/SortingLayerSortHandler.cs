using UnityEngine;
using UnityEngine.Rendering;

namespace App
{
    public class SortingLayerSortHandler : MonoBehaviour, ISortHandler
    {
        [SerializeField] private Canvas m_Canvas;

        public void SetDepth(int depth)
        {
            if (m_Canvas != null)
            {
                m_Canvas.overrideSorting = true;
                m_Canvas.sortingOrder = depth;
            }
            // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -depth);
        }
    }
}
