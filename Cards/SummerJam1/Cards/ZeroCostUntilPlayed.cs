using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class ZeroCostUntilPlayed : SummerJam1Component, IFreePlayCard
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                Entity.RemoveComponent(this);
            }
        }
    }
}