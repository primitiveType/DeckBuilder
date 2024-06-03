using Api;
using CardsAndPiles;

namespace SummerJam1.Cards
{
    public class BeatCost : SummerJam1Component, IAmount
    {
        public int Amount { get; set; }
        
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId != this.Entity)
            {
                return;
            }
            
            BeatCost BeatCost = args.CardId.GetComponent<BeatCost>();
            if (BeatCost.Amount == 0)
            {
                return;
            }
            Game.Battle.BeatTracker.AdvanceBeats(BeatCost.Amount);
        }
    }
}