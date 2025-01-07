using App;

namespace SummerJam1
{
    public class ShopPileView : PileView
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.Shop.Entity);
        }
    }
}