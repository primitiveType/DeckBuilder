using System;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ScaleY(this Vector3 vector, float value) => new Vector3(vector.x, vector.y * value, vector.z);
    public static Vector3 ScaleX(this Vector3 vector, float value) => new Vector3(vector.x * value, vector.y, vector.z);
    public static Vector3 ScaleZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, vector.z * value);
    public static Vector2 ScaleY(this Vector2 vector, float value) => new Vector2(vector.x, vector.y * value);
    public static Vector2 ScaleX(this Vector2 vector, float value) => new Vector2(vector.x * value, vector.y);
    public static Vector3 AddY(this Vector3 vector, float value) => new Vector3(vector.x, vector.y + value, vector.z);
    public static Vector3 AddX(this Vector3 vector, float value) => new Vector3(vector.x + value, vector.y, vector.z);
    public static Vector3 AddZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, vector.z + value);
    public static Vector2 AddY(this Vector2 vector, float value) => new Vector2(vector.x, vector.y + value);
    public static Vector2 AddX(this Vector2 vector, float value) => new Vector2(vector.x + value, vector.y);
    public static Vector3 WithY(this Vector3 vector, float value) => new Vector3(vector.x, value, vector.z);
    public static Vector3 WithX(this Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);
    public static Vector3 WithZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);
    public static Vector2 WithY(this Vector2 vector, float value) => new Vector2(vector.x, value);
    public static Vector2 WithX(this Vector2 vector, float value) => new Vector2(value, vector.y);
}

public static class CameraExtensions
{
    public static Bounds GetViewportBounds(this Camera camera, float distance)
    {
        if (camera.orthographic)
        {
            float frustumHeight = camera.orthographicSize * 2;
            float frustumWidth = frustumHeight * camera.aspect;
            return new Bounds(camera.transform.position, new Vector3(frustumWidth, frustumHeight));
        }
        else
        {
            //Untested.
            throw new NotSupportedException("This implementation is untested! remove this error and try it out.");
            float frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * camera.aspect;
            return new Bounds(camera.transform.position, new Vector3(frustumWidth, frustumHeight));
        }
    }
}

public static class RendererExtensions
{
    public static Vector3 ClampToViewport(this Renderer renderer, Camera camera)
    {
        Vector3 position = renderer.transform.position;
        Bounds viewportBounds = camera.GetViewportBounds(Vector3.Distance(position, camera.transform.position));
        Bounds encapsulated = viewportBounds;
        encapsulated.Encapsulate(renderer.bounds);

        Vector3 diff = encapsulated.center - viewportBounds.center;
        return position - (diff * 2);
    }
}