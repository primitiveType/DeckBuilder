using Api;

namespace SummerJam1
{
    public class SummerJam1Component : Component
    {
        protected new SummerJam1Events Events => (SummerJam1Events)base.Events;
        protected Game Game { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<Game>();
        }
    }
}