using App;

namespace SummerJam1
{
    public class EncounterDiscardPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.EncounterDiscardPile);
        }
    }
}