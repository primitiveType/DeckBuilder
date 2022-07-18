using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class ShowGameObjectOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform TooltipParent;
        [SerializeField] private float m_Delay = 1f;
        private bool Dragging { get; set; }


        private Coroutine ShowCoroutine { get; set; }


        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowCoroutine = StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            yield return new WaitForSeconds(m_Delay);
            TooltipParent.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ShowCoroutine != null)
            {
                StopCoroutine(ShowCoroutine);
                ShowCoroutine = null;
            }

            TooltipParent.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Dragging = true;
            if (ShowCoroutine != null)
            {
                StopCoroutine(ShowCoroutine);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Dragging = false;
        }
    }
}
