using System;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class DamageIntent : Intent
    {
        public int DamageAmount { get; set; }

        public override string GetDescription =>
            Context.GetDamageAmount(this, DamageAmount, TargetNode.Actor,
                Context.GetCurrentBattle().GetActorById(OwnerId)).ToString();

        private ActorNode TargetNode { get; set; }

        public override GameEntity Target => TargetNode;

        protected override void Initialize()
        {
            base.Initialize();
            var battle = Context.GetCurrentBattle();

            if (battle.GetAdjacentActors(battle.GetActorById(OwnerId)).Contains(battle.Player))
            {
                TargetNode = battle.GetNodeOfActor(battle.Player);
            }
            else
            {
                TargetNode = null;
            }
        }

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

            if (TargetNode?.Actor != null)
            { //SHould probably change this to actually target nodes
                Context.TryDealDamage(this, battle.GetActorById(OwnerId), TargetNode.Actor, DamageAmount);
            }
        }
    }
}