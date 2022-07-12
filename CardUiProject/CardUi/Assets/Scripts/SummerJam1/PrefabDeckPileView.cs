using App;

namespace SummerJam1
{
    public class PrefabDeckPileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(SummerJam1Context.Instance.Game.PrefabDebugPile.Entity);
        }
    }
}
