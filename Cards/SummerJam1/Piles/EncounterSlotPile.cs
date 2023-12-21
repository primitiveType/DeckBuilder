using System.Collections.Specialized;
using Api;
using CardsAndPiles;
using SummerJam1.Cards;

namespace SummerJam1.Piles
{
    public class EncounterSlotPile : DefaultPile
    {
        protected override int MaxChildren => 5;

        protected override void Initialize()
        {
            base.Initialize();
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }


        public override void Terminate()
        {
            base.Terminate();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }
}
