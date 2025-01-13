using CardsAndPiles;

namespace SummerJam1.Units
{
    public class DiesWhenAtZeroHealth : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Entity.TrySetParent(null);
            }
        }
    }
}