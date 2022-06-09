using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Common;
using SummerJam1;
using UnityEngine;

public class UnitRendererComponent : View<VisualComponent>
{
    // Start is called before the first frame update
    [PropertyListener(nameof(VisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        GameObject unitRenderer = SummerJam1UnitFactory.Instance.GetInstance(Model.AssetName);
        unitRenderer.transform.SetParent(transform);
        unitRenderer.transform.localPosition = new Vector3();
    }
}