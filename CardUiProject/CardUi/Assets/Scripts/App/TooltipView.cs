using System.Collections;
using System.Collections.Generic;
using App.Utility;
using CardsAndPiles.Components;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Api.Component;

namespace App
{
    public class TooltipView : View<Component>, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject TooltipPrefab;
        [SerializeField] private Transform TooltipParent;
        [SerializeField] private Collider TooltipBounds;
        [SerializeField] private float m_Delay = 1f;
        private bool Dragging { get; set; }


        private Coroutine ShowCoroutine { get; set; }

        private void Update()
        {
            if (TooltipParent.gameObject.activeInHierarchy)
            {
                if (TooltipBounds != null)
                {
                    TooltipParent.transform.localPosition = Vector3.zero;

                    Vector3 position = TooltipParent.transform.position;
                    Bounds tester = new Bounds(position, TooltipBounds.bounds.size);

                    Vector3 newCenter = tester.ClampToViewport(Camera.main);
                    position = newCenter.WithZ(position.z);
                    TooltipParent.transform.position = position;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowCoroutine = StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            yield return new WaitForSeconds(m_Delay);
            TooltipParent.gameObject.SetActive(true);

            List<ITooltip> components = Entity.GetComponents<ITooltip>();
            foreach (ITooltip component in components)
            {
                if (component.Tooltip == null)
                {
                    continue;
                }

                GameObject tooltip = Instantiate(TooltipPrefab, TooltipParent);
                tooltip.GetComponentInChildren<TMP_Text>().text = component.Tooltip;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ShowCoroutine != null)
            {
                StopCoroutine(ShowCoroutine);
                ShowCoroutine = null;
            }

            TooltipParent.gameObject.SetActive(false);
            foreach (Transform child in TooltipParent)
            {
                Destroy(child.gameObject);
            }
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
