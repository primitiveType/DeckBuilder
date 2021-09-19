using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float DragSpeed = 1f;
    [SerializeField] private float MinCamSize = 1f;
    [SerializeField] private float MaxCamSize = 5f;
    [SerializeField] private float ZoomIncrement = .5f;
    [SerializeField] private Camera Camera;
    private Vector3 lastPos;

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            lastPos = Mouse.current.position.ReadValue();
        }
        else if (Mouse.current.rightButton.isPressed)
        {
            Vector3 newPos = Mouse.current.position.ReadValue();
            Vector3 delta = Camera.ScreenToWorldPoint(lastPos) - Camera.ScreenToWorldPoint(newPos);
            Transform myTransform = Camera.transform;

            myTransform.position = myTransform.position + new Vector3(delta.x * DragSpeed, delta.y * DragSpeed);
            lastPos = newPos;
        }

        var scroll = Mouse.current.scroll.ReadValue();
        if (scroll.y > 0)
        {
            DoZoom(-1);
        }
        else if (scroll.y < 0)
        {
            DoZoom(1);
        }
    }

    private void DoZoom(int multiplier)
    {
        if (multiplier == 0)
        {
            return;
        }

        //track the old mouse position so we can zoom towards the mouse pointer.
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.ScreenToWorldPoint(mousePosition);
        Camera.orthographicSize =
            Mathf.Clamp(Camera.orthographicSize + ZoomIncrement * multiplier, MinCamSize, MaxCamSize);
        Vector3 newMouseWorldPosition = Camera.ScreenToWorldPoint(mousePosition);
        var delta = mouseWorldPosition - newMouseWorldPosition;
        Transform myTransform = Camera.transform;
        myTransform.position = myTransform.position + new Vector3(delta.x, delta.y);
    }
}