using App;

namespace SummerJam1
{
    public class BeatTrackerView : View<BeatTracker>
    {
        protected void Awake()
        {
            
            Game game = GameContext.Instance.Context.Root.GetComponent<Game>();
            BeatTracker beatTracker = game.Battle.BeatTracker;
            SetModel(beatTracker.Entity);
        }
    }
}
