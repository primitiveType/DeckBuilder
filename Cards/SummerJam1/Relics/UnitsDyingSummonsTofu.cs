using CardsAndPiles;
using SummerJam1.Units;

namespace SummerJam1.Relics
{
    public class UnitsDyingSummonsTofu : SummerJam1Component
    {
        private int AmountNeeded { get; } = 10;
        public int AmountRecorded { get; set; }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity.GetComponentInParent<FriendlyUnitSlot>() != null)
            {
                AmountRecorded++;
            }

            if (AmountRecorded >= AmountNeeded)
            {
                AmountRecorded = 0;
                var slot = Game.Battle.GetFrontMostEmptySlot();
                Context.CreateEntity(slot, "Tofu.json");
            }
        }
    }
}