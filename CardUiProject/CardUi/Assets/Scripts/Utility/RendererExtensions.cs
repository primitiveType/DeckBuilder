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
}