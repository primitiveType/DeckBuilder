using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class GainHealthWhenDrawn : SummerJam1Component, IAmount, IDescription
    {
        public int Amount { get; set; }

        public string Description => $"Gains {Amount} health each time it is drawn.";

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.DrawnCard != Entity)
            {
                return;
            }

            Entity.GetComponent<Health>().Max += Amount;
            Entity.GetComponent<Health>().Amount += Amount;
        }
    }
}