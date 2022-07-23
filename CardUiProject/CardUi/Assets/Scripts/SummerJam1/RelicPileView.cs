using App;

namespace SummerJam1
{
    public class RelicPileView : PileView
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.RelicPile.Entity);
        }
    }
}
