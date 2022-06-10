using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using App;
using Common;
using SummerJam1;
using UnityEngine;

public class UnitRendererComponent : View<VisualComponent>
{
    private GameObject CurrentRenderer { get; set; }

    // Start is called before the first frame update
    [PropertyListener(nameof(VisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        AnimationQueue.Instance.Enqueue(() =>
        {
            if (CurrentRenderer != null)
            {
                Destroy(CurrentRenderer);
            }

            CurrentRenderer = SummerJam1UnitFactory.Instance.GetInstance(Model.AssetName);
            CurrentRenderer.transform.SetParent(transform);
            CurrentRenderer.transform.localPosition = new Vector3();
            return null;
        });
    }
}