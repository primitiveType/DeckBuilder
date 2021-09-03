using System;
using System.Linq;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class MoveIntent : Intent
    {
        public override string GetDescription => "Move to an empty adjacent slot if there is one.";

        private ActorNode TargetNode { get; set; }

        public override GameEntity Target => TargetNode;

        protected override void Initialize()
        {
            base.Initialize();
            var battle = Context.GetCurrentBattle();

            TargetNode = battle.GetAdjacentEmptyNodes(battle.GetActorById(OwnerId)).FirstOrDefault();
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

            IActor actorById = battle.GetActorById(OwnerId);
            TargetNode = battle.GetAdjacentEmptyNodes(actorById).FirstOrDefault();

            if (TargetNode != null)
            {
                battle.MoveIntoSpace(actorById, TargetNode);
            }
        }
    }
}