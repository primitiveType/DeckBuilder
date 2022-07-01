using System.Collections;
using System.Collections.Generic;
using App.Utility;
using UnityEngine;

public class ClampToViewport : MonoBehaviour
{
    [SerializeField] private Collider m_Collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        transform.position = new Vector3(-10_000, -10_000, position.z);
        Bounds tester = m_Collider.bounds;
        Bounds heck = new Bounds(new Vector3(-10_000, -10_000, 0), tester.size);
        var newCenter = heck.ClampToViewport(Camera.main);
        position = newCenter.WithZ(position.z);
        transform.position = position;
    }
}
