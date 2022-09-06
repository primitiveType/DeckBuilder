using System.Collections.Specialized;
using System.Linq;
using App;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class VisibleIfEncounterSlotEmptyAndUpcomingNotEmpty : View<EncounterSlotPile>
    {
        private int Index { get; set; }
        [SerializeField] private Transform m_DeactivateWhenActive;

        protected override void Start()
        {
            base.Start();
            Index = GetComponentInParent<EncounterSlotPileView>().SlotNum;
            GameContext.Instance.Game.Battle.EncounterSlots[Index].Entity.Children.CollectionChanged += SlotChildrenOnCollectionChanged;
            UpdateVisibility();
        }

        private void SlotChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            bool active = !GameContext.Instance.Game.Battle.EncounterSlots[Index].Entity.Children.Any() &&
                          GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[Index].Entity.Children.Any();

            gameObject.SetActive(active);

            m_DeactivateWhenActive.gameObject.SetActive(!active);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameContext.Instance.Game.Battle.EncounterSlots[Index].Entity.Children.CollectionChanged -= SlotChildrenOnCollectionChanged;
        }
    }
}
