using System.Collections.Specialized;
using Api;
using CardsAndPiles;
using SummerJam1.Cards;

namespace SummerJam1.Piles
{
    public class EncounterSlotPile : DefaultPile
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged; 
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var count = Entity.Children.Count;
            int index = 0;
            foreach (IEntity entityChild in Entity.Children)
            {
                if (index == count - 1)
                {
                    if (entityChild.RemoveComponent<FaceDown>())
                    {
                        entityChild.
                    }
                    break;
                }

                index++;
                entityChild.GetOrAddComponent<FaceDown>();
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }

}
