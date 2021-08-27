using System;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class DamageIntent : Intent
    {
        public int DamageAmount { get; set; }

        public override string GetDescription =>
            Context.GetDamageAmount(this, DamageAmount, Target, Context.GetCurrentBattle().GetActorById(OwnerId)).ToString();

        private IActor Target => Context.GetCurrentBattle().Player;

        protected override void Trigger()
        {
            var battle = Context.GetCurrentBattle();
            if (OwnerId == battle.Player.Id)
            {
                throw new NotImplementedException("Player intents not implemented.");
            }

            if (OwnerId == -1)
            {
                throw new NotSupportedException("Intent with no owner was triggered!");
            }

            Context.TryDealDamage(this, battle.GetActorById(OwnerId), Target, DamageAmount);
        }
    }
}