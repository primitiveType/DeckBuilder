using System;
using CardsAndPiles;

namespace SummerJam1.Cards
{
    public class EnergyCost : SummerJam1Component
    {
        public int Cost { get; set; } 
        private SummerJam1Game Game { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<SummerJam1Game>();
        }

        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.CanPlay.Add(Game.Player.CurrentEnergy >= Cost);
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
}