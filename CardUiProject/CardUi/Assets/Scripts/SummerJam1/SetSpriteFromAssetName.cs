using System.ComponentModel;
using App;
using SummerJam1;
using UnityEngine;

public abstract class SetSpriteFromAssetName : View<VisualComponent>
{
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

            SetSprite(tex);
        }));
    }

    protected abstract void SetSprite(Sprite tex);
}
