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
            int count = Entity.Children.Count;
            int index = 0;
            foreach (IEntity entityChild in Entity.Children)
            {
                if (index == count - 1)
                {
                    //Aggro component indicates this monster was already awakened once. 
                    //This pattern is kinda bad so might have to revisit this with some sort of tryAddComponent or something.
                    if (entityChild.RemoveComponent<FaceDown>() && entityChild.GetComponent<Aggro>() == null)
                    {
                        entityChild.AddComponent<Asleep>();
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
