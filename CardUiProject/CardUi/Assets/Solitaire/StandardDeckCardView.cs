using Api;
using Common;
using Solitaire;
using UnityEngine;

public class StandardDeckCardView : PileItemView<StandardDeckCard>
{
    public Suit Suit => Model.Suit;
    public int Number => Model.Number;

    [SerializeField] private SpriteRenderer SpriteRenderer;


    protected override void OnInitialized()
    {
        base.OnInitialized();
        if(SpriteRenderer)
        {
            SpriteRenderer.sprite = SolitaireHelper.Instance.GetCardSprite(Number, Suit);
        }

        gameObject.name = Model.GetName();
    }
}