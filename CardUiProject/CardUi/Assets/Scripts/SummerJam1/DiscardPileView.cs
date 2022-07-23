using App;

namespace SummerJam1
{
    public class DiscardPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.Discard);
        }
    }
}