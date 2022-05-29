using System;
using UnityEngine;

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