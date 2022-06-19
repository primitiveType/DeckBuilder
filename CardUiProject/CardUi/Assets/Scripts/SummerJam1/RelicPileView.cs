using App;

namespace SummerJam1
{
    public class RelicPileView : PileView
    {
        protected void Awake()
        {
            SetModel(SummerJam1Context.Instance.Game.RelicPile.Entity);
        }
    }
}
