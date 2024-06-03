using App;

namespace SummerJam1
{
    public class HandPileView : PileView
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.Hand);
        }
    }
}
