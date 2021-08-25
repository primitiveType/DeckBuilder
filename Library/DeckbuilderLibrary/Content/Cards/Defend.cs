using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class Defend : EnergyCard
    {
        public override string Name => nameof(Defend);

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }

        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }

        private int BlockAmount = 5;

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Gain {Context.GetBlockAmount(this, BlockAmount, target as IActor, Owner)} to target enemy.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return null;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity _)
        {
            // Gain x block.
            Context.TryApplyBlock(this, Owner, Owner, BlockAmount);
        }

        public override int EnergyCost => 1;
    };
}