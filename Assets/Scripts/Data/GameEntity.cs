using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data
{
    public abstract class GameEntity
    {
        [JsonProperty] public int Id { get; private set; } = -1;

        [JsonIgnore] public IContext Context { get; private set; }
        public Properties Properties { get; protected set; } = new Properties();

        protected GameEntity(IContext context)
        {
            Context = context;
            Id = Context.GetNextEntityId();
            Context.AddEntity(this);
        }

        
        [JsonConstructor]
        protected GameEntity(int id, Properties properties)
        {
            Id = id;
            Properties = properties;
            Context = GameContext.CurrentContext;
            Context.AddEntity(this);

        }
    }

  
}