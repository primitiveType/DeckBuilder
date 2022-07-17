using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class Noisy : SummerJam1Component, ITooltip, IAmount, IDescription
    {
        public int Amount { get; set; }
        public string Description => $"Noise {Amount}.";

        public string Tooltip => "Noise - This card reduces stealth when played.";

        [OnCardPlayed]
        private void CardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (!Game.Player.TryUseStealth(Amount))
                { //its ok to play a noisy card when not stealthed.
                }
            }
        }
    }
}
