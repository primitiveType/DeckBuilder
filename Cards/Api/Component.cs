using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace Api
{
   
    public abstract class Component : IComponent
    {
        protected Context Context => Entity.Context;
        private List<EventHandle> EventHandles { get; } = new List<EventHandle>();
        [JsonIgnore] public IEntity Entity { get; private set; }
        private bool Initialized { get; set; }
        private Lazy<EventsBase> LazyEvents { get; }
        protected EventsBase Events => LazyEvents.Value;

        protected Component()
        {
            LazyEvents = new Lazy<EventsBase>(GetEvents);
        }

        private EventsBase GetEvents()
        {
            return Entity.GetComponentInParent<EventsBase>();
        }

        public void InternalInitialize(IEntity parent)
        {
            if (Initialized)
            {
                return;
            }

            Initialized = true;
            Entity = parent;
            //get attributes on each component.
            var type = GetType();
            foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public |
                                                          BindingFlags.Instance))
            {
                foreach (EventsBaseAttribute attribute in method.GetCustomAttributes<EventsBaseAttribute>())
                {
                    EventHandles.Add(attribute.GetEventHandle(method, this, Entity.GetComponentInParent<EventsBase>()));
                }
            }

            Initialize();
        }

        protected virtual void Initialize()
        {
        }

        public virtual void Terminate()
        {
            foreach (var eventHandle in EventHandles)
            {
                eventHandle.Dispose();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}