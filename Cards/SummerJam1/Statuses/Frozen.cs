using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class Frozen : SummerJam1Component, ITooltip
    {
        private bool CardDrawn { get; set; }

        public string Tooltip => "Frozen. This card cannot be played until drawn and discarded.";

        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.Blockers.Add(CardBlockers.FROZEN);
            }
        }

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.DrawnCard == Entity)
            {
                CardDrawn = true;
            }
        }

        [OnCardDiscarded]
        private void OnCardDiscarded(object sender, CardDiscardedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (CardDrawn)
                {
                    Entity.RemoveComponent(this);
                }
            }
        }
    }
}