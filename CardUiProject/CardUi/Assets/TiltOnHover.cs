using App;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.EventSystems;

public class TiltOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public BoxCollider Collider { get; set; }
    private bool isHovered;

    [SerializeField] private float xAmount;
    [SerializeField] private float yAmount;

    private Vector3 mousePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider = GetComponent<BoxCollider>();
        Item = GetComponent<IPileItemView>();
    }

    public IPileItemView Item { get; set; }


    // Update is called once per frame
    void Update()
    {
        var x = 0.0f;
        var y = 0.0f;
        if (isHovered)
        {
            var delta = transform.InverseTransformPoint(mousePosition) - (Collider.center);
            var percentage = new Vector3(delta.x / Collider.bounds.extents.x, delta.y / Collider.bounds.extents.y,
                delta.z / Collider.bounds.extents.z);

            x = xAmount * percentage.y;
            y = yAmount * -percentage.x;
        }

        var rot = Quaternion.Euler(x, y, 0);

        Item.SetTargetRotation(rot.eulerAngles);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        mousePosition = GetMousePosition(eventData);
    }

    private Vector3 GetMousePosition(PointerEventData eventData)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Mathf.Abs(transform.position.z - (eventData.enterEventCamera?.transform.position.z ?? 0 ))));
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.enterEventCamera != null)
        {
            mousePosition = GetMousePosition(eventData);
        }
    }
}