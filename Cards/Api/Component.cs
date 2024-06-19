using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Api
{
    public class DontSerializeAttribute : Attribute
    {
    }
    [DontSerialize]
    public class UnknownComponent : Component{}
    public abstract class Component : IEventfulComponent
    {
        protected Component()
        {
            LazyEvents = new Lazy<EventsBase>(GetEvents);
        }

        protected Context Context => Entity.Context;
        private List<IDisposable> EventHandles { get; } = new();

        [JsonIgnore] public LifecycleState State { get; private set; }
        private Lazy<EventsBase> LazyEvents { get; }
        protected EventsBase Events => LazyEvents.Value;
        [JsonIgnore] public IEntity Entity { get; private set; }
        public bool Enabled { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public void InternalInitialize(IEntity parent, bool attachEvents = true)
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

            if (!attachEvents)
            {
                State = LifecycleState.Initialized;
                return;
            }

            //get attributes on each component.
            Type type = GetType();
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

            foreach (IDisposable eventHandle in EventHandles)
            {
                eventHandle.Dispose();
            }

            State = LifecycleState.Destroyed;
        }

        [JsonIgnore]
        public Dictionary<int, int> EventEntrance { get; } = new(0);
    }
}
