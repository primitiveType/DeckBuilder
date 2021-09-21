using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards
{
    public class TheSoularium : EnergyCard
    {
        public override string Name => "The Soularium";
        private List<Card> DrawnCards => new List<Card>();

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
            Context.Events.TurnEnded += EventsOnTurnEnded;
        }

        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }

        private void EventsOnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            var cardsInHand = Context.GetCurrentBattle().Deck.HandPile.Cards;
            foreach (var card in DrawnCards)
            {
                if (!cardsInHand.Contains(card))
                {
                    continue;
                }

                Context.TrySendToPile(card.Id, PileType.ExhaustPile);
            }

            Context.Events.TurnEnded -= EventsOnTurnEnded;
        }

        private int DrawAmount = 3;

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Draw {Context.GetDrawAmount(this, DrawAmount, target as IActor, Owner)} cards. If they are in hand at the end of your turn, exhaust them.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return null;
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => false;
        public override int EnergyCost => 1;

        protected override void DoPlayCard(IGameEntity _)
        {
            var deck = Context.GetCurrentBattle().Deck;
            var drawPile = deck.DrawPile;
            for (int i = 0; i < Context.GetDrawAmount(this, DrawAmount, Owner, Owner); i++)
            {
                // Todo: Replace with a better draw API
                if (drawPile.Cards.Count > 0)
                {
                    // Oh no! I'm using Deck's TrySendToPile instead of Context's!
                    var card = deck.DrawPile.Cards[0];
                    deck.TrySendToPile(card, PileType.HandPile);
                    DrawnCards.Add(card);
                }
            }
        }
    }
}