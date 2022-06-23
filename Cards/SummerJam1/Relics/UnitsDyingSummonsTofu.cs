using CardsAndPiles;
using SummerJam1.Units;

namespace SummerJam1.Relics
{
    public class UnitsDyingSummonsTofu : SummerJam1Component
    {
        private int AmountNeeded { get; } = 5;
        public int AmountRecorded { get; set; }

        private bool ShouldSummon { get; set; }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity.GetComponentInParent<FriendlyUnitSlot>() != null)
            {
                AmountRecorded++;
            }

            if (AmountRecorded >= AmountNeeded)
            {
                ShouldSummon = true;
                AmountRecorded = 0;
            }
        }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            if (ShouldSummon)
            {
                ShouldSummon = false;
                var slot = Game.Battle.GetFrontMostEmptySlot();
                Context.CreateEntity(slot, "Units/TofuUnit.json");
            }
        }
    }
}
