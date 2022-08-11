using App;
using CardsAndPiles;

namespace SummerJam1
{
    public class EncounterDeckPileView : PileView
    {

        protected void Awake()
        {
            SetModel(GameContext.Instance.Game.Battle.EncounterDrawPile);
        }
    }
}
