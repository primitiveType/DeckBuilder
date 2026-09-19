using Api;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class EncounterSlotPileView : PileView
    {
        protected override IEntity GetEntityForView()
        {
            return GameContext.Instance.Game.Battle.MonsterSlots.Entity;
        }
    }
}
