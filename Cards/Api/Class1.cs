using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api
{
    public class EventHandle : IDisposable
    {
        private Action OnDispose { get; set; }

        public EventHandle(Action onDispose)
        {
            OnDispose = onDispose;
        }

        public void Dispose()
        {
            OnDispose.Invoke();
            OnDispose = null;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Entity
    {
        [JsonProperty] public int Id { get; private set; }

        [JsonProperty] public List<Component> Components { get; } = new List<Component>();

        public Entity Parent { get; private set; }

        public IReadOnlyList<Entity> Children => m_Children;
        [JsonProperty] private List<Entity> m_Children = new List<Entity>();

        internal void Initialize() //game context paramater?
        {
            //call generated code that does reflection to get all event attributes and subscribes to proper events
            //should also iterate components? OR should it only happen in components? depends on whether this stays sealed.

            foreach (var component in Components)
            {
                component.Initialize(this);
            }
        }

        internal void Terminate()
        {
            //dispose event handles we created from initialize.
            foreach (var component in Components)
            {
                component.Terminate();
            }
        }

        public void AddChild(Entity entity)
        {
            m_Children.Add(entity);
            entity.Parent = this;
            entity.Initialize();
        }

        public bool RemoveChild(Entity entity)
        {
            return m_Children.Remove(entity);
        }

        public void SetParent(Entity parent)
        {
            parent.AddChild(this);
        }

        public T GetComponent<T>()
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        public T GetComponentInParent<T>()
        {
            T component = default(T);
            var entity = Parent;
            while (entity != null)
            {
                component = entity.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }

                entity = entity.Parent;
            }

            return component;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var t = new T();
            Components.Add(t);

            return t;
        }

        public bool RemoveComponent(Component toRemove)
        {
            if (!Components.Remove(toRemove))
            {
                return false;
            }

            toRemove.Terminate();
            return true;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var component in Components)
            {
                component.Initialize(this);
            }

            foreach (var child in Children)
            {
                child.Parent = (this);
            }
        }
    }


    public abstract class Component
    {
        private List<EventHandle> EventHandles { get; } = new List<EventHandle>();
        [JsonIgnore]
        public Entity Parent { get; private set; }

        public void Initialize(Entity parent)
        {
            Parent = parent;
            //get attributes on each component.
            var type = GetType();
            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public |
                                                   BindingFlags.Instance))
            {
                foreach (EventAttribute attribute in method.GetCustomAttributes<EventAttribute>())
                {
                    EventHandles.Add(attribute.GetEventHandle(method, this, Parent.GetComponentInParent<EventsBase>()));
                }
            }
        }

        public void Terminate()
        {
            foreach (var eventHandle in EventHandles)
            {
                eventHandle.Dispose();
            }
        }
    }

    public class Card : Component
    {
    }

    public class Context : Component
    {
    }
}