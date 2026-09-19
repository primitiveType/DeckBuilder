using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App
{
    public class TooltipTester : MonoBehaviour, ITooltips, IPointerEnterHandler, IPointerExitHandler
    {
        public List<string> tooltips;
        public IReadOnlyList<string> GetTooltips()
        {
            return tooltips;
        }

        public Bounds GetTooltipBounds()
        {
            var image = GetComponent<Image>();
            Vector3[] corners = new Vector3[4];
            image.rectTransform.GetWorldCorners(corners);
            var bounds = new Bounds(corners[0], Vector3.one);

            foreach (var corner in corners)
            {
                bounds.Encapsulate(corner);
            }

            return bounds;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Hover.");
            TooltipManager.Instance.StartHover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.StopHover(this);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

    }
}