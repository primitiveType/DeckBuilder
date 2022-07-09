using App;

namespace SummerJam1
{
    public class PlayerView : View<Player>, ISetModel
    {
        protected override void Start()
        {
            base.Start();
            SetModel(SummerJam1Context.Instance.Context.Root.GetComponent<Game>().Player.Entity);
        }
    }
}
