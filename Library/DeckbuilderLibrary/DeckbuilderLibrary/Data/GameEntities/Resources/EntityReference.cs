using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public class EntityReference : IGameEntity, IInternalInitialize
    {
        [JsonIgnore]
        public int Id => Entity.Id;

        [JsonIgnore]
        public IContext Context { get; set; }
        private IGameEntity m_Entity;

        [JsonIgnore]
        public IGameEntity Entity
        {
            get => m_Entity;
            set
            {
                m_Entity = value;
                if (Entity == null)
                {
                    EntityId = -1;
                }
                else
                {
                    EntityId = Entity.Id;
                }
            }
        }

        [JsonProperty] public int EntityId { get; set; } = -1;

        public EntityReference()
        {
            ((IInternalGameContext)GameContext.CurrentContext).ToInitializeAdd(this);
        }

        public EntityReference(IGameEntity entity)
        {
            Entity = entity;
        }

        public void InternalInitialize()
        {
            if (Entity == null )
            {
                if (EntityId == -1)
                {
                    return;
                }
                Entity = GameContext.CurrentContext.GetCurrentBattle().GetActorById(EntityId);
            }
            else
            {
                EntityId = Entity.Id;
            }
        }

        public void SetContext(IContext context)
        {
            Context = context;
        }
    }
}