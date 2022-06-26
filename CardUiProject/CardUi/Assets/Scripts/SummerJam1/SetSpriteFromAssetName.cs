using System.ComponentModel;
using App;
using SummerJam1;
using UnityEngine;
using UnityEngine.UI;

public class SetSpriteFromAssetName : View<VisualComponent>
{
    private Image CurrentRenderer { get; set; }

    private void Awake()
    {
        CurrentRenderer = GetComponent<Image>();
    }

    // Start is called before the first frame update
    [PropertyListener(nameof(VisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        Debug.Log($"Getting texture for : {Model.AssetName}!");

        Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        {
            Sprite tex = Resources.Load<Sprite>(Model.AssetName);
            if (tex == null)
            {
                Debug.LogWarning($"No texture found for : {Model.AssetName}!");
            }

            CurrentRenderer.sprite = tex;
        }));
    }
}
