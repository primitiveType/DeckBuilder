using App;
using SummerJam1.Relics;

namespace SummerJam1
{
    public class BagHandPileView : PileView
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.Player.Entity.GetComponent<BagOfHolding>().Pile);
        }
    }
}