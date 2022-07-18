using App;

namespace SummerJam1
{
    public class DiscardPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(SummerJam1Context.Instance.Game.Battle.Discard);
        }
    }
}