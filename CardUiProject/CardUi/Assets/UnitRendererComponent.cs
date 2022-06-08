using System.Collections;
using System.Collections.Generic;
using SummerJam1;
using UnityEngine;

public class UnitRendererComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var unitRenderer = SummerJam1Helper.Instance.GetUnitRenderer();
        unitRenderer.transform.SetParent(transform);
        unitRenderer.transform.localPosition = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
    }
}