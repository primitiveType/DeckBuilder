using App;

namespace SummerJam1
{
    public class EncounterHandPileView : PileView
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.EncounterHandPile);
        }
    }
}
