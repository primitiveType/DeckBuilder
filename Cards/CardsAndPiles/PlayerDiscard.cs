using Api;

namespace CardsAndPiles
{
    public class PlayerDiscard : NotifiedPile
    {
        protected override void OnCardEnteredPile(IEntity eNewItem)
        {
            Events.OnCardDiscarded(new CardDiscardedEventArgs(eNewItem));
        }

        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
