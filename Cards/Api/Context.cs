using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api
{
    public class Context
    {
        [JsonProperty] private IEntity m_Root { get; set; }
        private int NextId { get; set; }

        public EventsBase Events { get; }

        public IEntity Root => m_Root;

        public Context(EventsBase events)
        {
            Events = events;
            m_Root = CreateEntity();
        }
        //create entity
        //initialize it
        //set its parent
        //add components
        //initialize components


        //Parent should not be available in initialize. Instead, listen to parent changed.
        //Components can be added at any time and will get initialized.


        public IEntity CreateEntity(IEntity parent = null, Action<IEntity> setup = null)
        {
            var entity = new Entity();
            entity.Initialize(this, NextId++);

            setup?.Invoke(entity);
            entity.TrySetParent(parent);
            return entity;
        }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitializeRecursively(Root);
            
            void InitializeRecursively(IEntity root)
            {
                ((Entity)root).Initialize(this, root.Id);
                foreach (IEntity child in root.Children)
                {
                    InitializeRecursively(child);
                }
            }
            
        }

      
    }
}