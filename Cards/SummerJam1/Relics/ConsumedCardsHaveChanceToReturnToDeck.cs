using CardsAndPiles;

namespace SummerJam1.Relics
{
    public class ConsumedCardsHaveChanceToReturnToDeck : SummerJam1Component
    {
        [OnCardExhausted]
        private void OnCardExhausted(object sender, CardExhaustedEventArgs args)
        {
            int roll = Game.Random.SystemRandom.Next(0, 10);
            if (roll > 4)
            {
                args.CardId.TrySetParent(Game.Battle.BattleDeck.Entity);
            }
        }
    }
}