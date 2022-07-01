using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api
{
    public class Context
    {
        public Context(EventsBase events)
        {
            Events = events;
            Root = CreateEntity();
        }

        [JsonProperty] public IEntity Root { get; private set; }
        [JsonProperty] private int NextId { get; set; }

        [JsonProperty] public EventsBase Events { get; private set; }

        public string PrefabsPath { get; private set; }


        public void SetPrefabsDirectory(string path)
        {
            PrefabsPath = path;
        }

        public IEntity CreateEntity(IEntity parent = null, Action<IEntity> setup = null)
        {
            Entity entity = new Entity();
            entity.Initialize(this, NextId++);

            setup?.Invoke(entity);
            entity.TrySetParent(parent);
            return entity;
        }

        public IEntity CreateEntity(IEntity parent, string prefabName)
        {
            string prefab = File.ReadAllText(Path.Combine(PrefabsPath, prefabName));
            Entity entity = Serializer.Deserialize<Entity>(prefab);
            entity.Initialize(this, NextId++);

            entity.TrySetParent(parent);
            return entity;
        }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitializeRecursively(Root);
        }

        private void InitializeRecursively(IEntity root)
        {
            ((Entity)root).Initialize(this, root.Id);
            foreach (IEntity child in root.Children)
            {
                InitializeRecursively(child);
            }
        }

        public IEntity DuplicateEntity(IEntity entity)
        {
            string copyStr = Serializer.Serialize(entity);

            Entity copy = Serializer.Deserialize<Entity>(copyStr);
            InitializeRecursively(copy);

            return copy;
        }
    }
}
