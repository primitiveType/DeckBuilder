using System.Collections.Specialized;
using Api;

namespace SummerJam1
{
    public abstract class Encounter : SummerJam1Component, IVisual
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.Parent.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity item in e.NewItems)
                {
                    if (item.GetComponent<Player>() != null)
                    {
                        PlayerEnteredCell();
                    }
                }
            }
        }

        protected abstract void PlayerEnteredCell();


        public override void Terminate()
        {
            base.Terminate();
            Entity.Parent.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }
}
