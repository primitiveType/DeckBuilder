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

        public void SetAllButTopFaceDown()
        {
            int index = 0;
            int count = Entity.Children.Count;
            foreach (IEntity entityChild in Entity.Children)
            {
                if (index == count - 1)
                {
                    entityChild.AddComponent<Aggro>();
                    break;
                }

                entityChild.AddComponent<FaceDown>();
                index++;
            }
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //when a card is covered, it is asleep.
            //when it is uncovered for the first time, it is aggro'd. and turned face up. and put to sleep until turn start.
            //


            int count = Entity.Children.Count;
            int index = 0;
            foreach (IEntity entityChild in Entity.Children)
            {
                //Aggro component indicates this monster was already revealed once. 
                //This pattern is kinda bad so might have to revisit this with some sort of tryAddComponent or something.
                bool isAggro = entityChild.GetComponent<Aggro>() != null;

                if (index == count - 1)
                {
                    if (entityChild.RemoveComponent<FaceDown>() && !isAggro)
                    {
                        entityChild.GetOrAddComponent<Asleep>();
                        entityChild.AddComponent<Aggro>();
                    }

                    break;
                }

                index++;
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }
}
