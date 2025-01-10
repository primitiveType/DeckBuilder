using System.Collections.Generic;
using CardsAndPiles.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Api.Component;

namespace App
{
    public class TooltipView : View<Component>, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IEndDragHandler, ITooltips
    {
        private bool Dragging { get; set; }

        private List<string> tooltips = new();

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.StartHover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.StopHover(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Dragging = true;
            TooltipManager.Instance.StopHover(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Dragging = false;
        }

        public IReadOnlyList<string> GetTooltips()
        {
            List<ITooltip> components = Entity.GetComponents<ITooltip>();
            tooltips.Clear();
            foreach (ITooltip component in components)
            {
                if (component.Tooltip == null)
                {
                    continue;
                }

                tooltips.Add(component.Tooltip);
            }

            return tooltips;
        }

        private Vector3[] corners = new Vector3[4];

        public Bounds GetTooltipBounds()
        {
            var rt = GetComponent<RectTransform>();
            var canvas = GetComponentInParent<Canvas>();

            if (canvas == null || canvas.renderMode == RenderMode.WorldSpace)
            {
                rt.GetWorldCorners(corners);
                var bounds = new Bounds(Camera.main.WorldToScreenPoint(corners[0]), Vector3.one);

                foreach (var corner in corners)
                {
                    bounds.Encapsulate(Camera.main.WorldToScreenPoint(corner));
                }

                return bounds;
            }
            else
            {
                rt.GetWorldCorners(corners);
                var bounds = new Bounds(corners[0], Vector3.one);

                foreach (var corner in corners)
                {
                    bounds.Encapsulate(corner);
                }

                return bounds;
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TooltipManager.Instance.StopHover(this);
        }
    }

    public interface ITooltips
    {
        IReadOnlyList<string> GetTooltips();
        Bounds GetTooltipBounds();

        Vector3 GetPosition();
    }
}