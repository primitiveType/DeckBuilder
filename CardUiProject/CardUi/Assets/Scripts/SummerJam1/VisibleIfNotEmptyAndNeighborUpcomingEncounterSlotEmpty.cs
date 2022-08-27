using System.Collections.Specialized;
using System.Linq;
using App;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class VisibleIfNotEmptyAndNeighborUpcomingEncounterSlotEmpty : View<EncounterSlotPile>
    {
        private int Index { get; set; }

        protected override void Start()
        {
            base.Start();
            Index = GetComponentInParent<EncounterSlotPileView>().SlotNum;
            if (Index >= GameContext.Instance.Game.Battle.EncounterSlotsUpcoming.Count - 1)
            { //last slot will never use this.
                gameObject.SetActive(false);
                return;
            }

            GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[Index].Entity.Children.CollectionChanged += SlotChildrenOnCollectionChanged;
            GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[Index + 1].Entity.Children.CollectionChanged += SlotChildrenOnCollectionChanged;
            UpdateVisibility();
        }

        private void SlotChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            //Visible if we are not empty, but our neighbor is.
            gameObject.SetActive(GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[Index].Entity.Children.Any() &&
                                 !GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[Index + 1].Entity.Children.Any());
        }
    }
}
