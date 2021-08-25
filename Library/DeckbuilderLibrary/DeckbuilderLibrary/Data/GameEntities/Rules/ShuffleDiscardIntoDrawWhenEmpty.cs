using System.Linq;
using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Rules
{
    public class ShuffleDiscardIntoDrawWhenEmpty : GameEntity
    {
        private IDeck Deck { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            Deck = Context.GetCurrentBattle().Deck;
            //In most games, this actually is checked when the player tries to draw a card. Will have to change this.
            Context.Events.CardMoved += OnCardMoved;
        }

        private void OnCardMoved(object sender, CardMovedEventArgs args)
        {
            if (Deck.DrawPile.Cards.Count == 0)
            {
                Context.Events.CardMoved -= OnCardMoved;
                foreach (Card discardPileCard in Deck.DiscardPile.Cards.ToList())
                {
                    Deck.TrySendToPile(discardPileCard, PileType.DrawPile);
                }
                Context.Events.CardMoved += OnCardMoved;
            }
        }
    }
}