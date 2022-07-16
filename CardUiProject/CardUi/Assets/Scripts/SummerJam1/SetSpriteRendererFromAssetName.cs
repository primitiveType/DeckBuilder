using UnityEngine;

public class SetSpriteRendererFromAssetName : SetSpriteFromAssetName
{
    private SpriteRenderer CurrentRenderer { get; set; }

    private void Awake()
    {
        CurrentRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void SetSprite(Sprite tex)
    {
        CurrentRenderer.sprite = tex;
    }
}
