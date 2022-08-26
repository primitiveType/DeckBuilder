using System.Collections.Specialized;
using Api;

namespace CardsAndPiles
{
    public abstract class NotifiedPile : Pile
    {
        protected new CardEvents Events => (CardEvents)base.Events;

        protected override void Initialize()
        {
            base.Initialize();
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity eNewItem in e.NewItems)
                {
                    OnCardEnteredPile(eNewItem);
                }
            }
        }

        protected abstract void OnCardEnteredPile(IEntity eNewItem);

        public override void Terminate()
        {
            base.Terminate();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }

        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
