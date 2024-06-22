using Api;
using App;

namespace SummerJam1
{
    public class DiscardPileView : PileView
    {
        protected override IEntity GetEntityForView()
        {
            return GameContext.Instance.Game.Battle.Discard;
        }
    }
}