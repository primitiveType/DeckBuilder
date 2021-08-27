using System.Linq;
using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Rules
{
    public class ShuffleDiscardIntoDrawWhenEmpty : GameEntity
    {
        private IBattleDeck BattleDeck => Context.GetCurrentBattle().Deck;
        protected override void Initialize()
        {
            base.Initialize();
            //In most games, this actually is checked when the player tries to draw a card. Will have to change this.
            Context.Events.CardMoved += OnCardMoved;
        }

        private void OnCardMoved(object sender, CardMovedEventArgs args)
        {
            if (BattleDeck.DrawPile.Cards.Count == 0)
            {
                Context.Events.CardMoved -= OnCardMoved;
                foreach (Card discardPileCard in BattleDeck.DiscardPile.Cards.ToList())
                {
                    BattleDeck.TrySendToPile(discardPileCard, PileType.DrawPile);
                }
                Context.Events.CardMoved += OnCardMoved;
            }
        }
    }
}