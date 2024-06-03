using UnityEngine;

public class ClampRectTransformToViewport : MonoBehaviour
{
    private RectTransform _canvas;
    private RectTransform _transform;
    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>().rootCanvas.gameObject.GetComponent<RectTransform>();
        _transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // RectTransform canvas, panel;
        var sizeDelta = _canvas.sizeDelta - _transform.sizeDelta;
        var panelPivot = _transform.pivot;
        var position = _transform.position;
        position.x = Mathf.Clamp(position.x, -sizeDelta.x * panelPivot.x, sizeDelta.x * (1 - panelPivot.x));
        position.y = Mathf.Clamp(position.y, -sizeDelta.y * panelPivot.y, sizeDelta.y * (1 - panelPivot.y));
        _transform.anchoredPosition = position;
    }
}
