using System.Collections.Specialized;
using System.Linq;
using App;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class VisibleIfIndexOfEmptyEncounterSlotGreaterThanUpcomingIndex : View<EncounterSlotPile>
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

            for (int i = Index + 1; i < BattleContainer.NumEncounterSlotsPerFloor; i++)
            {
                GameContext.Instance.Game.Battle.EncounterSlots[i].Entity.Children.CollectionChanged += EncounterSlotChildrenOnCollectionChanged;
            }

            UpdateVisibility();
        }

        private void EncounterSlotChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateVisibility();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            for (int i = Index + 1; i < BattleContainer.NumEncounterSlotsPerFloor; i++)
            {
                GameContext.Instance.Game.Battle.EncounterSlots[i].Entity.Children.CollectionChanged -= EncounterSlotChildrenOnCollectionChanged;
            }
        }


        private void UpdateVisibility()
        {
            bool visible = false;
            for (int i = Index + 1; i < BattleContainer.NumEncounterSlotsPerFloor; i++)
            {
                visible |= !GameContext.Instance.Game.Battle.EncounterSlots[i].Entity.Children.Any();
            }

            //Visible if any encounter slot to the right is empty.
            gameObject.SetActive(visible);
        }
    }
}
