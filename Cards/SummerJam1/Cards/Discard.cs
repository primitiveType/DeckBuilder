using CardsAndPiles;

namespace SummerJam1.Cards
{
    public class Discard : SummerJam1Component
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.CardId.TrySetParent(Context.Root.GetComponent<SummerJam1Game>().Battle.Discard);
            }
        }
    }
}