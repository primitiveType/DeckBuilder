using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}

public class BackAndForthRotate : MonoBehaviour
{
    Transform localTrans;
    public float angle = -15f;
    public float speed = 1f;
    public Axis axis = Axis.Y;

    bool rotateActive = true;
    Quaternion ogLocalRot;

    public bool startOnAwake = false;

    void Awake()
    {
        localTrans = transform;
        ogLocalRot = transform.localRotation;
        if(startOnAwake)
            Activate();
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void Activate()
    {
        StopAllCoroutines();
        rotateActive = true;
        StartCoroutine(AnimateCR());
    }

    IEnumerator AnimateCR()
    {
        Quaternion start = ogLocalRot;
        Vector3 euler = start.eulerAngles;

        switch(axis)
        {
            case Axis.X:
                euler.x += angle;
                break;
            case Axis.Y:
                euler.y += angle;
                break;
            case Axis.Z:
                euler.z += angle;
                break;
        }

        Quaternion target = Quaternion.Euler(euler);

        float phase = 0f;
        while(rotateActive)
        {
            while(phase < 1f)
            {
                phase = Mathf.Clamp01(phase + Time.deltaTime * speed);
                localTrans.localRotation = Quaternion.Slerp(start, target, Easing.Quadratic.InOut(phase));
                yield return null;
            }
            phase = 0f;
            while(phase < 1f)
            {
                phase = Mathf.Clamp01(phase + Time.deltaTime * speed);
                localTrans.localRotation = Quaternion.Slerp(target, start, Easing.Quadratic.InOut(phase));
                yield return null;
            }
            phase = 0f;
        }   
    }
}
