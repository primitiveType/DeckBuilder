using App;
using UnityEngine;

namespace SummerJam1
{
    public class EncounterSlotPileView : PileView
    {
        [SerializeField] private int m_SlotNum;
        [SerializeField] private bool m_Upcoming;

        protected void Awake()
        {
            base.Start();
            if (m_Upcoming)
            {
                SetModel(GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[m_SlotNum]);
            }
            else
            {
                SetModel(GameContext.Instance.Game.Battle.EncounterSlots[m_SlotNum]);
            }
        }
    }
}
