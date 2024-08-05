using Api;
using App;

namespace SummerJam1
{
    public class PlayerUnitSlotPileView : PileView
    {
        protected override IEntity GetEntityForView()
        {
            return GameContext.Instance.Game.PlayerUnits.Entity;
        }
    }
}