using App;
using CardsAndPiles;
using UnityEngine;

namespace SummerJam1
{
    public class EncounterSlotPileView : View<IPile>, IPileView
    {
        [SerializeField] private int SlotNum;

        protected override void Start()
        {
            base.Start();
            SetModel(GameContext.Instance.Game.Battle.EncounterSlots[SlotNum]);
        }
    }
}