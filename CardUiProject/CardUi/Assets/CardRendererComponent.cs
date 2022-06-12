using System.ComponentModel;
using App;
using SummerJam1;
using UnityEngine;

public class CardRendererComponent : View<CardVisualComponent>
{
    private GameObject CurrentRenderer { get; set; }

    // Start is called before the first frame update
    [PropertyListener(nameof(CardVisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        {
            if (CurrentRenderer != null)
            {
                Destroy(CurrentRenderer);
            }


            CurrentRenderer = SummerJam1CardFactory.Instance.GetRenderer(Model.AssetName);
            CurrentRenderer.transform.SetParent(transform);
            CurrentRenderer.transform.localPosition = new Vector3();
            CurrentRenderer.transform.localScale =  Vector3.one;
            CurrentRenderer.transform.localRotation =  Quaternion.identity;
            
        }));
    }
}