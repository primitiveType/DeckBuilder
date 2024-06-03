using UnityEngine;
using UnityEngine.UI;

public class SetImageSpriteFromAssetName : SetSpriteFromAssetName
{
    private Image CurrentRenderer { get; set; }

    private void Awake()
    {
        CurrentRenderer = GetComponent<Image>();
    }

    protected override void SetSprite(Sprite tex)
    {
        CurrentRenderer.sprite = tex;
    }
}
