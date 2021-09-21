using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards
{
    public class TargetsCards : EnergyCard
    {
        
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += OnCardPlayed;
        }
        public override string Name => nameof(TargetsCards);
        public override int EnergyCost => 1;
        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => true;

        public override string GetCardText(IGameEntity target)
        {
            return "Choose a card in your hand. Discard it.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Deck.HandPile.Cards.Where(x => x.Id != Id).ToList();
        }
        protected override void DoPlayCard(IGameEntity target)
        {
            Context.TrySendToPile(target.Id, PileType.DiscardPile);
        }

    }
}