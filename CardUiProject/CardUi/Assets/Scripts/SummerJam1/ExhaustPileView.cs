using App;

namespace SummerJam1
{
    public class ExhaustPileView : PileView
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.Exhaust);
        }
    }
}
