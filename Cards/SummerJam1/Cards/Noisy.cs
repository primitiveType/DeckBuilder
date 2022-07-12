using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class Noisy : SummerJam1Component, ITooltip
    {
        [OnCardPlayed]
        private void CardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (!Game.Player.TryUseStealth(1))
                {//its ok to play a noisy card when not stealthed.
                }
            }
        }

        public string Tooltip => "Noisy - This card reduces stealth when played.";
    }
}
