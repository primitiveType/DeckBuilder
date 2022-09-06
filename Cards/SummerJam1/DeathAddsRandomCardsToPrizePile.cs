using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class DeathAddsRandomCardsToPrizePile : SummerJam1Component, IDescription
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Game.PrizePile.SetupRandomPrizePile();
            }
        }

        public string Description => "Destroy to gain a choice of cards.";
    }
}