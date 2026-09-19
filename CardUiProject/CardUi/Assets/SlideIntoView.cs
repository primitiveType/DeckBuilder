using UnityEngine;
using App.Utility;
using UnityEngine.Serialization;

public class SlideIntoView : MonoBehaviour
{
    [SerializeField] private Vector3 _slideVector;
    [SerializeField] private float lerpRate = 1f;
    [SerializeField] private bool active;

    private Vector3 currentTarget => active ? Vector3.zero : _slideVector;
    //assume we want to be at (0,0) LOCAL position.
    //apply slide vector when deactivating.

    private void Awake()
    {
        transform.localPosition = currentTarget;
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    public void Toggle()
    {
        active = !active;
    }

    void Update()
    {
        transform.localPosition =
            VectorExtensions.Damp(transform.localPosition, currentTarget, lerpRate, Time.deltaTime);
    }
}