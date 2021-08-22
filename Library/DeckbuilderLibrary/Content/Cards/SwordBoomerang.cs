using System.Collections.Generic;
using System;
using Data;

namespace Content.Cards
{
    public class SwordBoomerang : Card
    {
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
        private int DamageAmount = 6;
        private int RepeatAmount = 4;
        public override string Name => "Sword Boomerang";
        public override string GetCardText(IGameEntity target = null)
        {
            // Note by Snapper:
            //  I'd like to stick with a single interface, but I don't see Context.GetRepeatAmount(object sender, int baseRepeat, IActor target, IActor owner) really working
            //  It isn't clear how it would work as the card isn't played 4 times.
            // Todo(Arthur, Snapper): The card text doesn't actually match what the card does. There's no rule that says it stops casting when there are no more enemies. We probably need to make that the default behavior.
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to a random enemy {RepeatAmount}.";
        }
        
        // Todo(Arthur): This feel wrong. Any suggestions?
        public override IReadOnlyList<IActor> GetValidTargets()
        {
            IReadOnlyList<IActor> enemies = Context.GetEnemies();
            if (enemies.Count == 0)
            {
                return null;
            }
            Random random = new Random();
            int index = random.Next(enemies.Count);
            // Todo(Arthur): What's the idiomatic way to do this?
            return new List<IActor> {enemies[index]};
        }
        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IActor target)
        {
            // Deal x damage to y random enemies.
            for (int i = 0; i < RepeatAmount; i++)
            {
                IReadOnlyList<IActor> enemies = GetValidTargets();
                if (enemies == null)
                {
                    return;
                }
                Context.TryDealDamage(this, Owner, target, DamageAmount);
            }
        }
    }
}