using Api;

namespace CardsAndPiles
{
    public class PlayerExhaust : NotifiedPile
    {
        protected override void OnCardEnteredPile(IEntity eNewItem)
        {
            Events.OnCardExhausted(new CardExhaustedEventArgs(eNewItem));
        }

        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
