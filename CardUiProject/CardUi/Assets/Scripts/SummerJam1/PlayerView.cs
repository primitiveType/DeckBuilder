using App;

namespace SummerJam1
{
    public class PlayerView : View<Player>, ISetModel
    {
        protected void Awake()
        {
            SetModel(SummerJam1Context.Instance.Context.Root.GetComponent<Game>().Player.Entity);
        }
    }
}
