using System;
using System.Collections.Generic;
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

        public Dictionary<int, IEntity> EntityDatabase { get; } = new Dictionary<int, IEntity>();

        [JsonProperty] public IEntity Root { get; private set; }
        [JsonProperty] private int NextId { get; set; }

        [JsonProperty] public EventsBase Events { get; private set; }

        public string PrefabsPath { get; private set; }


        public void SetPrefabsDirectory(string path)
        {
            PrefabsPath = path;
        }

        //create
        //initialize
        //setup
        //parent

        public IEntity CreateEntity(IEntity parent = null, Action<IEntity> setup = null)
        {
            Entity entity = new Entity();
            entity.Initialize(this, NextId++);

            setup?.Invoke(entity);
            if (!entity.TrySetParent(parent))
            {
                throw new Exception("Unable to create entity under target!");
            }
            EntityDatabase.Add(entity.Id, entity);
            Events.OnEntityCreated(new EntityCreatedEventArgs(entity));
            return entity;
        }

        public IEntity CreateEntity(IEntity parent, string prefabName)
        {
            if (!prefabName.ToLower().EndsWith(".json"))
            {
                prefabName += ".json";
            }
            string prefab = File.ReadAllText(Path.Combine(PrefabsPath, prefabName));
            Entity entity = Serializer.Deserialize<Entity>(prefab);
            entity.Initialize(this, NextId++);

            entity.TrySetParent(parent);
            EntityDatabase.Add(entity.Id, entity);
            Events.OnEntityCreated(new EntityCreatedEventArgs(entity));
            return entity;
        }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitializeRecursively(Root);
        }

        //order is different while deserializing.
        //create
        //parent
        //initialize
        //-setup-
        private void InitializeRecursively(IEntity root)
        {
            ((Entity)root).Initialize(this, NextId++);
            EntityDatabase.Add(root.Id, root);

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
