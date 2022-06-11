﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api
{
    public enum LifecycleState
    {
        Created,
        Initialized,
        Destroyed
    }

    [JsonObject(MemberSerialization.OptIn)]
    internal class Entity : IEntity
    {
        public Context Context { get; private set; }
        [JsonProperty] public int Id { get; private set; } = -1;

        public IChildrenCollection<Component> Components => ComponentsInternal;

        [JsonProperty]
        private ChildrenCollection<Component> ComponentsInternal { get; set; } = new ChildrenCollection<Component>();

        public LifecycleState State { get; private set; }

        public IEntity Parent { get; private set; }

        public IChildrenCollection<IEntity> Children => m_Children;
        [JsonProperty] private ChildrenCollection<IEntity> m_Children = new ChildrenCollection<IEntity>();

        internal void Initialize(Context context, int id) //game context parameter?
        {
            if (State != LifecycleState.Created)
            {
                return;
            }

            Id = id;

            Context = context;

            Components.CollectionChanged += ComponentsOnCollectionChanged;
            //call generated code that does reflection to get all event attributes and subscribes to proper events
            //should also iterate components? OR should it only happen in components? depends on whether this stays sealed.

            foreach (var component in Components.ToList())
            {
                component.InternalInitialize(this);
            }

            State = LifecycleState.Initialized;
        }

        public void Destroy()
        {
            Terminate();
            SetParent(null);
            foreach (var child in Children)
            {
                child.Destroy();
            }
            State = LifecycleState.Destroyed;
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
            m_Children.Add(entity, () => { entity.Parent = this; });
        }

        public bool RemoveChild(Entity entity)
        {
            return m_Children.Remove(entity);
        }

        private void SetParent(IEntity newParent)
        {
            if (Parent == newParent)
            {
                return;
            }

            ((Entity)Parent)?.RemoveChild(this);

            if (newParent != null)
            {
                ((Entity)newParent).AddChild(this);
            }
            else
            {
                Parent = null;
            }
        }


        public bool TrySetParent(IEntity parent)
        {
            if (parent != null) //setting null is always valid.... ?
            {
                foreach (IParentConstraint component in GetComponents<IParentConstraint>())
                {
                    if (!component.AcceptsParent(parent))
                    {
                        return false;
                    }
                }


                foreach (IParentConstraint component in parent.GetComponents<IParentConstraint>())
                {
                    if (!component.AcceptsChild(this))
                    {
                        return false;
                    }
                }
            }

            SetParent(parent);
            return true;
        }

        public T GetComponent<T>()
        {
            return GetComponents<T>().FirstOrDefault();
        }
        
        
        public T GetOrAddComponent<T>() where T : Component, new()
        {
            T existing = GetComponent<T>();
            if (existing != null)
            {
                return existing;
            }

            return AddComponent<T>();
        }

        public List<T> GetComponents<T>()
        {
            return Components.OfType<T>().ToList();
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

            foreach (IEntity entity in Children)
            { //looks like depth first... I guess that's ok for now.
                component = entity.GetComponentInChildren<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return default(T);
        }

        public List<T> GetComponentsInChildren<T>()
        {
            List<T> components = new List<T>();

            DoGetComponentsInChildren(this);

            void DoGetComponentsInChildren(IEntity entity)
            {
                T component = entity.GetComponent<T>();
                if (component != null)
                {
                    components.Add(component);
                }


                foreach (IEntity child in entity.Children)
                {
                    DoGetComponentsInChildren(child);
                }
            }


            return components;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            if (State == LifecycleState.Initialized)
            {
                ComponentsInternal.Add(component, () => { component.InternalInitialize(this); });
            }
            else
            {
                ComponentsInternal.Add(component);
            }

            return component;
        }

        public bool RemoveComponent(Component toRemove)
        {
            if (!ComponentsInternal.Remove(toRemove))
            {
                return false;
            }

            return true;
        }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // foreach (Component component in Components)
            // {
            //     component.InternalInitialize(this);
            // }

            foreach (IEntity child in Children)
            {
                ((Entity)child).Parent = (this);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public delegate void EntityDestroyedEvent(object sender, EntityDestroyedEventArgs args);

    public class EntityDestroyedEventArgs
    {
    }

    public interface IParentConstraint
    {
        bool AcceptsParent(IEntity parent);
        bool AcceptsChild(IEntity child);
    }

    public interface IEntity : INotifyPropertyChanged
    {
        Context Context { get; }
        int Id { get; }
        IEntity Parent { get; }
        IChildrenCollection<IEntity> Children { get; }
        IChildrenCollection<Component> Components { get; }
        bool TrySetParent(IEntity parent);
        T GetComponent<T>();
        List<T> GetComponents<T>();
        T GetComponentInParent<T>();
        T GetComponentInChildren<T>();
        List<T> GetComponentsInChildren<T>();
        T AddComponent<T>() where T : Component, new();
        bool RemoveComponent(Component toRemove);
        LifecycleState State { get; }
        void Destroy();
        T GetOrAddComponent<T>() where T : Component, new();
    }
}