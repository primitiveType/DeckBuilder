using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Defend : Card
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

        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return null;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IActor target)
        {
            // Gain x block.
            Context.TryApplyBlock(this, Owner, target, BlockAmount);
        }
    };
}