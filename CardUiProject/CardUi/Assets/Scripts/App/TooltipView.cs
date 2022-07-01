using System.Collections.Generic;
using App.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Api.Component;

namespace App
{
    public class TooltipView : View<Component>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject TooltipPrefab;
        [SerializeField] private Transform TooltipParent;
        [SerializeField] private Collider TooltipBounds;

        private void Update()
        {
            if (TooltipParent.gameObject.activeInHierarchy)
            {
                TooltipParent.transform.localPosition = Vector3.zero;
                
                Vector3 position = TooltipParent.transform.position;
                Debug.Log($"position before {position}");
                Bounds tester = new Bounds(position, TooltipBounds.bounds.size);
                
                Vector3 newCenter = tester.ClampToViewport(Camera.main);
                position = newCenter.WithZ(position.z);
                TooltipParent.transform.position = position;
                Debug.Log($"position after {position}");

            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipParent.gameObject.SetActive(true);

            List<Component> components = Entity.GetComponents<Component>(); //add interface for tooltips?
            foreach (Component component in components)
            {
                GameObject tooltip = Instantiate(TooltipPrefab, TooltipParent);
                tooltip.GetComponentInChildren<TMP_Text>().text = component.GetType().Name;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipParent.gameObject.SetActive(false);
            foreach (Transform child in TooltipParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
