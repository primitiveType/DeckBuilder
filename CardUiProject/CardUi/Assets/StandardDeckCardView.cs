using Api;
using Solitaire;
using UnityEngine;

public class StandardDeckCardView : View<StandardDeckCard>
{
    public Suit Suit => Model.Suit;
    public int Number => Model.Number;

    [SerializeField] private SpriteRenderer SpriteRenderer;


    protected override void OnInitialized()
    {
        base.OnInitialized();
        SpriteRenderer.sprite = SolitaireHelper.Instance.GetCardSprite(Number, Suit);
        gameObject.name = $"{Number} of {Suit.ToString()}";
    }
}