using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        [JsonProperty] public IChildrenCollection<Component> Components => m_Components;
        [JsonProperty] private ChildrenCollection<Component> m_Components = new ChildrenCollection<Component>();


        public Entity Parent { get; private set; }

        public IChildrenCollection<Entity> Children => m_Children;
        [JsonProperty] private ChildrenCollection<Entity> m_Children = new ChildrenCollection<Entity>();

        internal void Initialize() //game context paramater?
        {
            Components.CollectionChanged += ComponentsOnCollectionChanged;
            //call generated code that does reflection to get all event attributes and subscribes to proper events
            //should also iterate components? OR should it only happen in components? depends on whether this stays sealed.

            foreach (var component in Components)
            {
                component.InternalInitialize(this);
            }
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Component added in e.NewItems)
                {
                    added.InternalInitialize(this);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Component removed in e.OldItems)
                {
                    removed.Terminate();
                }
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

        public T GetComponentInChildren<T>()
        {
            T component = GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            foreach (Entity entity in Children)
            { //looks like depth first... I guess that's ok for now.
                component = entity.GetComponentInChildren<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return component;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var t = new T();
            m_Components.Add(t);

            return t;
        }

        public bool RemoveComponent(Component toRemove)
        {
            if (!m_Components.Remove(toRemove))
            {
                return false;
            }

            return true;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var component in Components)
            {
                component.InternalInitialize(this);
            }

            foreach (var child in Children)
            {
                child.Parent = (this);
            }
        }
    }

    public class ChildrenCollection<T> : ObservableCollection<T>, IChildrenCollection<T>
    {
    }

    public interface IChildrenCollection<T> : IReadOnlyCollection<T>, INotifyCollectionChanged
    {
    }


    public abstract class Component
    {
        private List<EventHandle> EventHandles { get; } = new List<EventHandle>();
        [JsonIgnore] public Entity Parent { get; private set; }

        public void InternalInitialize(Entity parent)
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

            Initialize();
        }

        protected virtual void Initialize()
        {
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