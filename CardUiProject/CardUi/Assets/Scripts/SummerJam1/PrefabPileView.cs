using App;

namespace SummerJam1
{
    public class PrefabPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.PrefabsContainer);
        }
    }
}