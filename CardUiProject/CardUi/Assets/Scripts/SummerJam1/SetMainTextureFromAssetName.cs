using System.ComponentModel;
using Api;
using App;
using SummerJam1;
using UnityEngine;

public class SetMainTextureFromAssetName : View<VisualComponent>
{
    private Renderer CurrentRenderer { get; set; }

    private void Awake()
    {
        CurrentRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    [PropertyListener(nameof(VisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        Logging.Log($"Getting texture for : {Model.AssetName}!");

        Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        {
            var tex = (Texture)Resources.Load(Model.AssetName);
            if (tex == null)
            {
                Logging.LogWarning($"No texture found for : {Model.AssetName}!");
            }

            CurrentRenderer.material.mainTexture = tex;
        }));
    }
}
