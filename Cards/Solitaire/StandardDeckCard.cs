using System.ComponentModel;
using CardsAndPiles;
using Component = Api.Component;

namespace Solitaire
{
    public class StandardDeckCard : Component, IPileItem, IDraggable
    {
        public Suit Suit { get; private set; }
        public int Number { get; private set; }
        public bool IsFaceDown { get; set; }
        public bool CanDrag { get; set; } = true;

        public SuitColor SuitColor => Suit == Suit.Clubs || Suit == Suit.Spades ? SuitColor.Black : SuitColor.Red;


        public void SetCard(int number, Suit suit, bool isFaceDown = false)
        {
            Suit = suit;
            Number = number;
            IsFaceDown = isFaceDown;
        }

        //Should this just be an extension method?
        public bool TrySendToPile(IPile pile)
        {
            return pile.ReceiveItem(this);
        }

        public string GetName()
        {
            int cardNumber = Number + 2;
            string numberName = cardNumber.ToString();
            if (cardNumber == 11)
            {
                numberName = "Jack";
            }
            else if (cardNumber == 12)
            {
                numberName = "Queen";
            }
            else if (cardNumber == 13)
            {
                numberName = "King";
            }
            else if (cardNumber == 14)
            {
                numberName = "Ace";
            }

            return $"{numberName} of {Suit.ToString()}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}