using System;
using Api;
using CardsAndPiles;

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
}