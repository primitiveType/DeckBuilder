using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorNode : GameEntity
    {
        public IActor Actor { get; set; }

        internal void SetActorNoEvent(IActor actor)
        {
            Actor = actor;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.ActorDied += OnActorDied;
        }

        private void OnActorDied(object sender, ActorDiedEventArgs args)
        {
            if (Actor?.Id == args.Actor.Id)
            {
                Actor = null;
            }
        }
    }
}