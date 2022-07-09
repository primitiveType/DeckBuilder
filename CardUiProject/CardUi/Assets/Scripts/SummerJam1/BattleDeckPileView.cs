using App;

namespace SummerJam1
{
    public class BattleDeckPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(SummerJam1Context.Instance.Game.Battle.BattleDeck.Entity);
        }
    }
}
