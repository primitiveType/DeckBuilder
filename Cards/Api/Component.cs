using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Api
{
    public abstract class Component : IComponent
    {
        protected Context Context => Entity.Context;
        private List<IDisposable> EventHandles { get; } = new List<IDisposable>();
        [JsonIgnore] public IEntity Entity { get; private set; }
        public bool Enabled { get; set; } = true;

        [JsonIgnore] public LifecycleState State { get; private set; }
        private Lazy<EventsBase> LazyEvents { get; }
        protected EventsBase Events => LazyEvents.Value;

        protected Component()
        {
            LazyEvents = new Lazy<EventsBase>(GetEvents);
        }

        private EventsBase GetEvents()
        {
            EventsBase events = Context.Events;
            if (events == null)
            {
                throw new NullReferenceException(
                    $"Failed to find events for entity {Entity.Id}:{Entity.GetComponents<IComponent>().First().GetType().Name}!");
            }

            return events;
        }

        public void InternalInitialize(IEntity parent)
        {
            if (State != LifecycleState.Created)
            {
                return;
            }

            Entity = parent;

            if (Context.Events == null)
            {
                throw new NullReferenceException("No Events object found while initializing component!");
            }

            //get attributes on each component.
            var type = GetType();
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Default | BindingFlags.Public |
                                                          BindingFlags.Instance | BindingFlags.Static))
            {
                foreach (EventsBaseAttribute attribute in method.GetCustomAttributes<EventsBaseAttribute>(true))
                {
                    EventHandles.Add(attribute.GetEventHandle(method, this, Context.Events));
                }
            }

            //Private functions have to be handled separately due to a bug in Mono's implementation of GetMethods.
            while (type != typeof(object))
            {
                // ReSharper disable once PossibleNullReferenceException. Above condition guarantees it will not be null.
                foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic |
                                                              BindingFlags.Instance | BindingFlags.Static))
                {
                    foreach (EventsBaseAttribute attribute in method.GetCustomAttributes<EventsBaseAttribute>(true))
                    {
                        EventHandles.Add(attribute.GetEventHandle(method, this, Context.Events));
                    }
                }

                type = type.BaseType;
            }

            Initialize();
            State = LifecycleState.Initialized;
        }

        protected virtual void Initialize()
        {
        }

        public virtual void Terminate()
        {
            if (State != LifecycleState.Initialized)
            {
                return;
            }

            foreach (var eventHandle in EventHandles)
            {
                eventHandle.Dispose();
            }

            State = LifecycleState.Destroyed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
