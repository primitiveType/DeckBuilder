using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using Newtonsoft.Json;

namespace Data
{
    [JsonConverter(typeof(GameEntityConverter))]
    public abstract class GameEntity
    {
        [JsonProperty] public int Id { get; internal set; } = -1;

        [JsonIgnore] public IContext Context { get; internal set; }

        internal GameEntity()
        {
        }

        public virtual void Initialize()
        {
        }

        // public Properties Properties { get; protected set; } = new Properties();
    }
}