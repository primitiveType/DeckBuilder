using UnityEngine;

public class StandardDeckCard : MonoBehaviour
{
    public Suit Suit { get; private set; }
    public int Number { get; private set; }

    [SerializeField] private SpriteRenderer SpriteRenderer;
    
    public void SetCard(int number, Suit suit)
    {
        Suit = suit;
        Number = number;
        SpriteRenderer.sprite = SolitaireHelper.Instance.GetCardSprite( Number , Suit);
        gameObject.name = $"{Number} of {Suit.ToString()}";
    }
}