using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class ScaleSpriteRenderer : MonoBehaviour
{
    [SerializeField] private float pixelsPerUnit = 100f;
    [SerializeField] private Vector2 DesiredSize = Vector2.one;
    private void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        var sprite = sr.sprite;
        var texWidth = sprite.texture.width;
        var texHeight = sprite.texture.height;
        
        //if width is 500, it will be 5 meters. so we divide by five to get one meter.
        //so (desiredsize/(width / pixelsperunit)) 
        var desiredX = DesiredSize.x / (texWidth / pixelsPerUnit);
        var desiredY = DesiredSize.y / (texHeight / pixelsPerUnit);
        var desired = Mathf.Min(desiredX, desiredY);
        transform.localScale = new Vector3(desired, desired, desired);

    }
}
