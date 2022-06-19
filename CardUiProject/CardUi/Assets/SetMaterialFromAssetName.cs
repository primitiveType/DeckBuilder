using System.ComponentModel;
using App;
using SummerJam1;
using UnityEngine;

public class SetMaterialFromAssetName : View<VisualComponent>
{
    private Renderer CurrentRenderer { get; set; }

    private void Awake()
    {
        CurrentRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    [PropertyListener(nameof(CardVisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        {
            CurrentRenderer.material = (Material)Resources.Load(Model.AssetName);
        }));
    }
}