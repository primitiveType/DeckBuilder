using System.Linq;
using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Rules
{
    public class DiscardHandAtEndOfTurn : GameEntity
    {
        private IDeck Deck { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            Deck = Context.GetCurrentBattle().Deck;
            //In most games, this actually is checked when the player tries to draw a card. Will have to change this.
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            var hand = Deck.HandPile.Cards.ToList();
            foreach (var card in hand)
            {
                Context.TrySendToPile(card.Id, PileType.DiscardPile);
            }
        }

       
    }
}