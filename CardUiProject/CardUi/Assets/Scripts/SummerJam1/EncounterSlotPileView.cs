using App;
using UnityEngine;

namespace SummerJam1
{
    public class EncounterSlotPileView : PileView
    {
        [SerializeField] private int m_SlotNum;
        [SerializeField] private bool m_Upcoming;
        
        public int SlotNum => m_SlotNum;
        protected void Awake()
        {
            base.Start();
            if (m_Upcoming)
            {
                SetModel(GameContext.Instance.Game.Battle.EncounterSlotsUpcoming[SlotNum]);
            }
            else
            {
                SetModel(GameContext.Instance.Game.Battle.EncounterSlots[SlotNum]);
            }
        }
    }
}
