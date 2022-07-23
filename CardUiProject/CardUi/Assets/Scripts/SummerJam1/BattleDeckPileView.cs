using App;

namespace SummerJam1
{
    public class BattleDeckPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.BattleDeck.Entity);
        }
    }
}
