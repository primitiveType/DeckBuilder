using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using JetBrains.Annotations;
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
        private bool Initialized { get; set; }

        internal void Initialize() //game context paramater?
        {
            if (Initialized)
            {
                return;
            }
            Components.CollectionChanged += ComponentsOnCollectionChanged;
            //call generated code that does reflection to get all event attributes and subscribes to proper events
            //should also iterate components? OR should it only happen in components? depends on whether this stays sealed.

            foreach (var component in Components)
            {
                component.InternalInitialize(this);
            }

            Initialized = true;
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
            //Add to collection.
            //set parent./
            //initialize
            //fire added event.
            m_Children.Add(entity, () =>
            {
                entity.Parent = this;
                entity.Initialize();
            });
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
            T component = new T();
            if (Initialized)
            {
                m_Components.Add(component, () => { component.InternalInitialize(this); });
            }
            else
            {
                m_Components.Add(component);
            }

            return component;
        }

        public bool RemoveComponent(Component toRemove)
        {
            if (!m_Components.Remove(toRemove, toRemove.Terminate))
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

    public class ChildrenCollection<T> : INotifyCollectionChanged, IChildrenCollection<T>, ICollection<T>
    {
        [ItemNotNull] private ICollection<T> m_CollectionImplementation = new Collection<T>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        public IEnumerator<T> GetEnumerator()
        {
            return m_CollectionImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item, Action invokeBeforeEvent)
        {
            m_CollectionImplementation.Add(item);
            invokeBeforeEvent?.Invoke();
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item }));
        }

        public void Add(T item)
        {
            Add(item, null);
        }


        public void Clear()
        {
            var oldItems = m_CollectionImplementation.ToList();
            m_CollectionImplementation.Clear();
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
        }

        public bool Contains(T item)
        {
            return m_CollectionImplementation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_CollectionImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item, Action invokeBeforeEvent)
        {
            bool removed = m_CollectionImplementation.Remove(item);
            if (removed)
            {
                invokeBeforeEvent?.Invoke();
                CollectionChanged?.Invoke(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T> { item }));
            }

            return removed;
        }

        public bool Remove(T item)
        {
            return Remove(item, null);
        }

        public int Count => m_CollectionImplementation.Count;
        public bool IsReadOnly => m_CollectionImplementation.IsReadOnly;
    }

    public interface IChildrenCollection<T> : IReadOnlyCollection<T>, INotifyCollectionChanged
    {
    }


    public abstract class Component : IComponent
    {
        private List<EventHandle> EventHandles { get; } = new List<EventHandle>();
        [JsonIgnore] public Entity Parent { get; private set; }
        private bool Initialized { get; set; }

        public void InternalInitialize(Entity parent)
        {
            if (Initialized)
            {
                return;
            }

            Initialized = true;
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

    public interface IComponent
    {
    }

    public class Card : Component
    {
    }

    public class Context : Component
    {
    }
}