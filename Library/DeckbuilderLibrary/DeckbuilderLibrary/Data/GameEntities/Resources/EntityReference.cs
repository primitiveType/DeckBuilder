using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public class EntityReference<TGameEntity> : IGameEntity, IInternalInitialize where TGameEntity : IGameEntity
    {
        [JsonIgnore] public int Id => Entity.Id;

        [JsonIgnore] public IContext Context { get; set; }

        public void AddListener(Action<object, PropertyChangedEventArgs> action)
        {
            m_Entity.AddListener(action);
        }

        public event EntityDestroyed DestroyedEvent;

        private TGameEntity m_Entity;

        [JsonIgnore]
        public TGameEntity Entity
        {
            get => m_Entity;
            set
            {
                if (m_Entity != null)
                {
                    m_Entity.DestroyedEvent -= EntityOnDestroyedEvent;
                }

                m_Entity = value;
                if (Entity == null)
                {
                    EntityId = -1;
                }
                else
                {
                    EntityId = Entity.Id;
                    m_Entity.DestroyedEvent += EntityOnDestroyedEvent;
                }
            }
        }

        private void EntityOnDestroyedEvent(object sender, EntityDestroyedArgs args)
        {
            DestroyedEvent?.Invoke(sender, args);
        }

        [JsonProperty] public int EntityId { get; set; } = -1;

        public EntityReference()
        {
            ((IInternalGameContext)GameContext.CurrentContext).ToInitializeAdd(this);
        }

        public EntityReference(TGameEntity entity)
        {
            Entity = entity;
        }

        public void InternalInitialize()
        {
            if (Entity == null)
            {
                if (EntityId == -1)
                {
                    return;
                }

                Entity = (TGameEntity)GameContext.CurrentContext.GetCurrentBattle().GetActorById(EntityId);
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