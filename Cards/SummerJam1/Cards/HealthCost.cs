using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class HealthCost : SummerJam1Component, IDescription, IAmount
    {
        public int Amount { get; set; }

        public string Description => $"Lose {Amount} Health.";

        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (Game.Player.Entity.GetComponent<Health>().Amount <= Amount)
                {
                    args.Blockers.Add(CardBlockers.NOT_ENOUGH_HEALTH); //DEBUG
                }
            }
        }

        [OnCardPlayed]
        private void CardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (!args.IsFree)
                {
                    Game.Player.Entity.GetComponent<Health>().DealDamage(Amount, Entity);
                }
            }
        }
    }
}