using System;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public abstract class GameEntity : IInternalGameEntity
    {
        [JsonProperty] public int Id { get; internal set; } = -1;

        [JsonIgnore] public IContext Context { get; private set; }

        [JsonIgnore] private bool Initialized { get; set; }

        void IInternalGameEntity.InternalInitialize()
        {
            if (Initialized)
            {
                throw new NotSupportedException(
                    $"Attempted to initialize an already initialized Entity with id {Id}. This is not supported.");
            }

            Initialize();
            Initialized = true;
        }

        public void SetContext(IContext context)
        {
            if (Context != null)
            {
                throw new NotSupportedException(
                    $"Attempted to set context on an already initialized Entity with id {Id}. This is not supported.");
            }

            Context = context;
        }

        internal GameEntity()
        {
        }


        protected virtual void Initialize()
        {
        }

        // public Properties Properties { get; protected set; } = new Properties();
    }
}