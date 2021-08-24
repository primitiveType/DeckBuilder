using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.Events
{
    public class ActorDiedEventArgs
    {
        public ActorDiedEventArgs(Actor actor, IGameEntity causeOfDeath, IGameEntity causeOfDeathOwner)
        {
            Actor = actor;
            CauseOfDeath = causeOfDeath;
            CauseOfDeathOwner = causeOfDeathOwner;
        }

        public Actor Actor { get; }
        public IGameEntity CauseOfDeath { get; }
        public IGameEntity CauseOfDeathOwner { get; }
    }
}