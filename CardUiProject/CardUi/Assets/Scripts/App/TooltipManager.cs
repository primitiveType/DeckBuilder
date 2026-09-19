using System.Collections;
using System.Collections.Generic;
using App.Utility;
using CardsAndPiles.Components;
using TMPro;
using UnityEngine;

namespace App
{
    public class TooltipManager : MonoBehaviourSingleton<TooltipManager>
    {
        [SerializeField] private GameObject TooltipPrefab;
        [SerializeField] private Transform TooltipParent;
        [SerializeField] private float m_Delay = 1f;
        private Coroutine ShowCoroutine { get; set; }
        private ITooltips Source { get; set; }

        private void Update()
        {
            if (TooltipParent.gameObject.activeInHierarchy)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            if (Source != null)
            {
                var sourceBounds = Source.GetTooltipBounds();

                TooltipParent.transform.localPosition = Vector3.zero;

                var targetPosition = sourceBounds.max;
                var centerOfObject = sourceBounds.center;
                //works only in pixel coords...
                if (centerOfObject.x > Screen.width / 2f)
                {
                    targetPosition = sourceBounds.min;
                    TooltipParent.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                }
                else
                {
                    TooltipParent.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                }

                targetPosition.y = centerOfObject.y;
                TooltipParent.transform.position = targetPosition;
            }
        }

        public void StartHover(ITooltips source)
        {
            Source = source;
            ShowCoroutine = StartCoroutine(Show());
        }

        public void StopHover(ITooltips source)
        {
            if (source != Source)
            {
                return;
            }
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

        private IEnumerator Show()
        {
            yield return new WaitForSeconds(m_Delay);
            TooltipParent.gameObject.SetActive(true);
            UpdatePosition();
            var tooltips = Source.GetTooltips();
            foreach (string tip in tooltips)
            {
                GameObject tooltip = Instantiate(TooltipPrefab, TooltipParent);
                tooltip.GetComponentInChildren<TMP_Text>().text = tip;
            }
        }
    }
}