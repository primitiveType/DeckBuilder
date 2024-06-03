using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftXOverTime : MonoBehaviour
{
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;
    [SerializeField] private float TimeOffset;
    [SerializeField] private float TimeMultiplier = 1;
    [SerializeField] private RectTransform _rectTransform;


    void Update()
    {
        Vector3 position = _rectTransform.anchoredPosition;

        float t = Mathf.PerlinNoise(TimeMultiplier * Time.time + TimeOffset, 0);
        float x = Mathf.Lerp(MinX, MaxX, t);

        position.x = x;

        _rectTransform.anchoredPosition = position;
    }
}
