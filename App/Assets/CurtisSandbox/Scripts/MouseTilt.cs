using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseTilt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float MaxTilt = 15f;
    [SerializeField]
    private float RecenterSpeed = 5f;
    [SerializeField]
    private float SmoothRampMarginU = 0.1f;

    private float SmoothRampMarginV => SmoothRampMarginU * (Collider.bounds.extents.x / Collider.bounds.extents.y);



    [SerializeField]
    Transform m_Transform;
    Transform Transform => (m_Transform != null) ? m_Transform : m_Transform = transform;

    Collider m_Collider;
    Collider Collider => (m_Collider != null) ? m_Collider : m_Collider = GetComponent<Collider>();

    private bool MouseHovering { get; set; }

    private void Update()
    {
        if (MouseHovering)
        {
            OnMouseHover();
        }
    }

    private void OnMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Collider.Raycast(ray, out hit, 100.0f))
        {
            float uRotation = Mathf.Lerp(-MaxTilt, MaxTilt, hit.textureCoord.x);
            float vRotation = Mathf.Lerp(-MaxTilt, MaxTilt, hit.textureCoord.y);

            Quaternion tiltedRotation = Quaternion.Euler(vRotation, -uRotation, 0f);
            //Transform.localRotation = Quaternion.Lerp(Transform.localRotation, targetRotation, Time.deltaTime * RecenterSpeed);
            //Transform.localRotation = targetRotation;

            float uMarginDepth = GetUVMarginDepth(hit.textureCoord.x, SmoothRampMarginU);
            float vMarginDepth = GetUVMarginDepth(hit.textureCoord.y, SmoothRampMarginV);
            float tiltRatio = Mathf.Min(uMarginDepth, vMarginDepth);


            Quaternion targetRotation = Quaternion.Lerp(Quaternion.identity, tiltedRotation, tiltRatio);
            Transform.localRotation = Quaternion.Lerp(Transform.localRotation, targetRotation, Time.deltaTime * RecenterSpeed * 2f);
        }
    }

    private float GetUVMarginDepth(float coord, float rampMargin)
    {
        if (coord <= rampMargin)
        {
            return Mathf.InverseLerp(0f, rampMargin, coord);
        }
        else if (coord >= (1f - rampMargin))
        {
            return Mathf.InverseLerp(1f, (1f - rampMargin), coord);
        }

        return 1f;
    }



    private IEnumerator ReturnToCenterCR()
    {
        float phase = 0f;
        Quaternion startRotation = Transform.localRotation;

        while (phase < 1f)
        {
            phase = Mathf.Clamp01(phase + Time.deltaTime * RecenterSpeed);
            Transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, phase);
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        MouseHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(ReturnToCenterCR());
        MouseHovering = false;
    }
}
