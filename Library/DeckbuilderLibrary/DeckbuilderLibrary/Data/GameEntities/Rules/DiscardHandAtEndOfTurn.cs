using System.Linq;
using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Rules
{
    public class DiscardHandAtEndOfTurn : GameEntity
    {
        private IBattleDeck BattleDeck { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            BattleDeck = Context.GetCurrentBattle().Deck;
            //In most games, this actually is checked when the player tries to draw a card. Will have to change this.
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            var hand = BattleDeck.HandPile.Cards.ToList();
            foreach (var card in hand)
            {
                Context.TrySendToPile(card.Id, PileType.DiscardPile);
            }
        }

       
    }
}