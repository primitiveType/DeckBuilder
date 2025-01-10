using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Cards.Effects.Powers
{
    public class GainBlockWhenCardsGetBloodied : Power, IAmount, IDescription
    {
        public int Amount { get; set; } = 1;

        [OnCardGainedStatus]
        private void OnCardGainedStatus(object sender, CardGainedStatusEventArgs args)
        {
            if (PowerActive && args.Status == nameof(Bloodied))
            {
                Entity.GetOrAddComponent<Armor>().Amount += Amount;
            }
        }

        public string Description => $"Gain {Amount} Block when a card is bloodied.";
    }
}