using System;
using System.Collections.Specialized;
using Api;

namespace CardsAndPiles
{
    public abstract class Pile : Component, IPile, IParentConstraint

    {
        public virtual bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public abstract bool AcceptsChild(IEntity child);
    }

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

    public class PlayerDiscard : NotifiedPile
    {
        public PlayerDiscard()
        {
            Console.WriteLine("Created discard.");
        }

        protected override void OnCardEnteredPile(IEntity eNewItem)
        {
            Events.OnCardDiscarded(new CardDiscardedEventArgs(eNewItem));
        }

        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }

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
            foreach (IEntity eNewItem in e.NewItems)
            {
                OnCardEnteredPile(eNewItem);
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