using App;
using UnityEngine;

namespace SummerJam1
{
    public class EncounterSlotPileView : PileView
    {
        [SerializeField] private int SlotNum;

        protected void Awake()
        {
            base.Start();
            SetModel(GameContext.Instance.Game.Battle.EncounterSlots[SlotNum]);
        }
    }
}
