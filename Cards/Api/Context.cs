using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api
{
    public class Context
    {
        //create
        //initialize
        //setup
        //parent

        public delegate void SetupBeforeParenting(IEntity newChild);

        public Context(EventsBase events)
        {
            Events = events;
            Root = CreateEntity();
        }

        public Dictionary<int, IEntity> EntityDatabase { get; } = new();


        [JsonProperty] public IEntity Root { get; private set; }
        [JsonProperty] private int NextId { get; set; }

        [JsonProperty] public EventsBase Events { get; private set; }

        public static string PrefabsPath { get; private set; }


        public void SetPrefabsDirectory(string path)
        {
            PrefabsPath = path;
        }

        public TComponent CreateEntity<TComponent>(IEntity parent = null) where TComponent : Component, new()
        {
            TComponent component = null;
            CreateEntity(parent, child => component = child.AddComponent<TComponent>());

            return component;
        }

        public IEntity CreateEntity(IEntity parent = null, SetupBeforeParenting setup = null)
        {
            Entity entity = new();
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

        public IEntity CreateEntity(IEntity parent, string prefabName, Action<IEntity> setup = null, bool shouldInitialize = true)
        {
            if (!prefabName.ToLower().EndsWith(".json"))
            {
                prefabName += ".json";
            }

            string prefab = File.ReadAllText(Path.Combine(PrefabsPath, prefabName));
            Entity entity = Serializer.Deserialize<Entity>(prefab);
            entity.GetOrAddComponent<SourcePrefab>().Prefab = prefabName;
            if (shouldInitialize)
            {
                entity.Initialize(this, NextId++);
                EntityDatabase.Add(entity.Id, entity);
            }

            setup?.Invoke(entity);

            if (!entity.TrySetParent(parent))
            {
                throw new Exception($"Failed to parent entity during creation! {prefabName}");
            }

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
