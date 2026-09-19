using System.ComponentModel;
using Api;
using App;
using SummerJam1;
using UnityEngine;

public abstract class SetSpriteFromAssetName : View<VisualComponent>
{
    [PropertyListener(nameof(VisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {

        // Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        // {
        Sprite tex = Resources.Load<Sprite>(Model.AssetName);
        if (tex == null)
        {
            Logging.LogWarning($"No texture found for : {Model.AssetName}!");
        }

        SetSprite(tex);
        // }));
    }

    protected abstract void SetSprite(Sprite tex);
}
