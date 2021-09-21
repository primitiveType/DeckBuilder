using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards
{
    public class SecondChance : EnergyCard
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += OnCardPlayed;
        }

        public override string Name => nameof(SecondChance);
        public override int EnergyCost => 1;

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => true;

        public override string GetCardText(IGameEntity target)
        {
            return "Choose a card from your exhaust. Add it to your hand.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Deck.ExhaustPile.Cards;
        }

        protected override void DoPlayCard(IGameEntity target)
        {
            Context.TrySendToPile(target.Id, PileType.HandPile);
        }
    }
}