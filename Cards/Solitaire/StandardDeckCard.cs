using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class StandardDeckCard : Component, IPileItem
    {
        public Suit Suit { get; private set; }
        public int Number { get; private set; }


        public void SetCard(int number, Suit suit)
        {
            Suit = suit;
            Number = number;
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
    }
}