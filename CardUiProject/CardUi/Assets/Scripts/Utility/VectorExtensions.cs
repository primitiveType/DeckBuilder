using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public static float Damp(float a, float b, float lambda, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}