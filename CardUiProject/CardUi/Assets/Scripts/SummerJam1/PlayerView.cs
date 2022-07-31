using App;

namespace SummerJam1
{
    public class PlayerView : View<Player>, ISetModel
    {
        protected void Awake()
        {
            SetModel(GameContext.Instance.Context.Root.GetComponent<Game>().Player.Entity);
            CameraManager.Instance.SetFollowTarget(gameObject);
        }
    }
}
