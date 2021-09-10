using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorNode : GameEntity
    {
        private EntityReference ActorEntityReference { get; set; }= new EntityReference();

        [JsonIgnore]
        public IActor Actor
        {
            get => ActorEntityReference.Entity as IActor;
            set => ActorEntityReference.Entity = value;
        }

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