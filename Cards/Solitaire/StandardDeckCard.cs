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
    }
}