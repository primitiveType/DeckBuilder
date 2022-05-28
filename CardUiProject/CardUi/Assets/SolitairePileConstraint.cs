using System.Linq;
using UnityEngine;

public class SolitairePileConstraint : MonoBehaviour, IPileConstraint
{
    private IPile Pile { get; set; }
    private void Awake()
    {
        Pile = GetComponentInParent<IPile>();
    }

    public bool CanReceive(IPileItem item)
    {
        var card = item.gameObject.GetComponent<StandardDeckCard>();

        return card != null && SuitCompatible(card.Suit) && IsNextSequence(card.Number);
    }

    private bool IsNextSequence(int number)
    {
        if (Pile.Items.Last().gameObject.GetComponent<StandardDeckCard>().Number == number - 1)
        {
            return true;
        }

        return false;
    }

    private bool SuitCompatible(Suit suit)
    {
        if (Pile.Items.Count == 0)
        {
            return true;
        }

        return Pile.Items.First().gameObject.GetComponent<StandardDeckCard>().Suit == suit;
    }
}