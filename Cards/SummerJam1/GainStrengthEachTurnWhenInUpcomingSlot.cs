using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class GainStrengthEachTurnWhenInUpcomingSlot : SummerJam1Component, IAmount, IDescription
    {
        public int Amount { get; set; }

        public string Description => $"Gains {Amount} Strength each turn spent above the board.";


        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (Entity.GetComponentInParent<UpcomingEncounterSlotPile>() != null)
            {
                Entity.GetOrAddComponent<Strength>().Amount += Amount;
            }
        }
    }
}