using App;

namespace SummerJam1
{
    public class PrefabDeckPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.PrefabDebugPileTester.Entity);
        }
    }
}
