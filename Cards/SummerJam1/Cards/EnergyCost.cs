using System;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class EnergyCost : SummerJam1Component
    {
        public int Cost { get; set; }


        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            if (Game.Player.CurrentEnergy < Cost)
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
                    if (!Game.Player.TryUseEnergy(Cost))
                    {
                        throw new Exception("Somehow played a card without the required energy!");
                    }
                }
            }
        }
    }

    public class HealthCost : SummerJam1Component
    {
        public int Cost { get; set; }


        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if(Game.Player.Entity.GetComponent<Health>().Amount <= Cost)
                {
                    args.Blockers.Add(CardBlockers.NOT_ENOUGH_HEALTH);
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
                    Game.Player.Entity.GetComponent<Health>().DealDamage(Cost, Entity);
                }
            }
        }
    }
}
