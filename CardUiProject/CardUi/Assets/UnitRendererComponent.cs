using System.ComponentModel;
using App;
using SummerJam1;
using SummerJam1.Units;
using UnityEngine;

public class UnitRendererComponent : View<UnitVisualComponent>
{
    private GameObject CurrentRenderer { get; set; }

    // Start is called before the first frame update
    [PropertyListener(nameof(UnitVisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        {
            if (CurrentRenderer != null)
            {
                Destroy(CurrentRenderer);
            }


            CurrentRenderer = SummerJam1UnitFactory.Instance.GetInstance(Model.AssetName);
            CurrentRenderer.transform.SetParent(transform, false);
            CurrentRenderer.transform.localPosition = new Vector3();
            if (Entity.GetComponentInParent<FriendlyUnitSlot>() != null)
            {
                var scale = CurrentRenderer.transform.localScale;
                CurrentRenderer.transform.localScale = new Vector3(-1 * scale.x, scale.y, scale.z);
            }
        }));
    }
}