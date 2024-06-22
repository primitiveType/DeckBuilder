using Api;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class EncounterSlotPileView : PileView
    {
        [SerializeField] private int m_SlotNum;

        public int SlotNum => m_SlotNum;
        
        protected override IEntity GetEntityForView()
        {
            return GameContext.Instance.Game.Battle.EncounterSlots.Entity;
        }
    }
}
