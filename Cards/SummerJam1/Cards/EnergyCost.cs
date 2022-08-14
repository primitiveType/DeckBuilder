using System;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class EnergyCost : SummerJam1Component, IAmount
    {
        public int Amount { get; set; }


        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            if (Game.Player.CurrentEnergy < Amount)
            {
                args.Blockers.Add(CardBlockers.NOT_ENOUGH_ENERGY);
            }
        }

        [OnCardPlayed]
        private void CardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (!args.IsFree)
                {
                    if (!Game.Player.TryUseEnergy(Amount))
                    {
                        throw new Exception("Somehow played a card without the required energy!");
                    }
                }
            }
        }
    }

    public class HealthCost : SummerJam1Component, IDescription, IAmount
    {

        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if(Game.Player.Entity.GetComponent<Health>().Amount <= Amount)
                {
                    args.Blockers.Add(CardBlockers.NOT_ENOUGH_HEALTH);//DEBUG
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

        public string Description => $"Lose {Amount} Health.";
        public int Amount { get; set; }
    }
}
