using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class BodySlam : Card
    {
        public override string Name => "Body Slam";
        private int DamageAmount = 0;
        protected override void Initialize()
        {
            
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
            
        }
        protected override void DoPlayCard(IActor target)
        {
            // Deal damage equal to your block.
            Context.TryDealDamage(this, Owner, target, DamageAmount + Owner.Armor);
        }
        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }
        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return Context.GetEnemies();
        }
        public override bool RequiresTarget => true;
        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal damage equal to your block.";
        }

    }
}