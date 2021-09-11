using System;
using System.Linq;
using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class MoveIntent : Intent
    {
        public int MovementAmount { get; set; } = 3;
        public override string GetDescription => "Move to an empty adjacent slot if there is one.";


        public override GameEntity Target => null;

        protected override void Initialize()
        {
            base.Initialize();
            var battle = Context.GetCurrentBattle();
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
            var path = new ActorNodePath(battle.Graph.GetNodeOfActor(actorById),
                battle.Graph.GetNodeOfActor(battle.Player));


            int moves = 0;
            foreach (var node in path)
            {
                
                if (moves >= MovementAmount)
                    break;

                
                battle.Graph.MoveIntoSpace(actorById, node);
                moves++;
            }
        }
        
        
        
    }
}