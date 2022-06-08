using UnityEngine;

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
    
    public static Vector3 ClampToViewport(this Bounds bounds, Camera camera)
    {
        Bounds viewportBounds = camera.GetViewportBounds(Vector3.Distance(bounds.center, camera.transform.position));
        Bounds encapsulated = viewportBounds;
        encapsulated.Encapsulate(bounds);

        Vector3 diff = encapsulated.center - viewportBounds.center;
        return bounds.center - (diff * 2);
    }
}