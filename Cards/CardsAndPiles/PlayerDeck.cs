using Api;

namespace CardsAndPiles
{
    public class PlayerDeck : NotifiedPile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }

        protected override void OnCardEnteredPile(IEntity eNewItem)
        {
        }
    }
}
