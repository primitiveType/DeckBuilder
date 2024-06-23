using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Api
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{GetComponent<IComponent>()?.GetType()?.Name ?? \"Entity\"}")]
    internal class Entity : IEntity
    {
        [JsonProperty] private ChildrenCollection<IEntity> _children = new();

        [JsonProperty] private ChildrenCollection<Component> ComponentsInternal { get; set; } = new();
        public Context Context { get; private set; }
        [JsonProperty] public int Id { get; private set; } = -1;

        public IChildrenCollection<Component> Components => ComponentsInternal;

        public LifecycleState State { get; private set; }

        public IEntity Parent { get; private set; }

        public IChildrenCollection<IEntity> Children => _children;

        public void Destroy()
        {
            Terminate();
            SetParent(null);
            foreach (IEntity child in Children.ToList())
            {
                child.Destroy();
            }

            State = LifecycleState.Destroyed;
        }


        public bool TrySetParent(IEntity parent)
        {
            if (!CanSetParent(parent))
            {
                return false;
            }

            SetParent(parent);
            return true;
        }

        public bool CanSetParent(IEntity parent)
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

            return true;
        }

        public T GetComponent<T>()
        {
            return GetComponents<T>().FirstOrDefault();
        }
        
        public object GetComponent(Type type)
        {
            return Components.FirstOrDefault(c => c.GetType().IsAssignableFrom(type));
        }

        public bool HasComponent<T>()
        {
            return Components.OfType<T>().Any();
        }
        
        public bool HasComponent(Type type)
        {
            return Components.Any(c => c.GetType().IsAssignableFrom(type));
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
            T component = default;
            IEntity entity = Parent;
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

        public T GetComponentInSelfOrParent<T>()
        {
            T component = default;
            IEntity entity = this;
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

            return default;
        }

        public List<T> GetComponentsInChildren<T>()
        {
            List<T> components = new();

            DoGetComponentsInChildren(this);

            void DoGetComponentsInChildren(IEntity entity)
            {
                components.AddRange(entity.GetComponents<T>());

                foreach (IEntity child in entity.Children)
                {
                    DoGetComponentsInChildren(child);
                }
            }


            return components;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new();
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
        
        public IComponent AddComponent(Type type)
        {
            Component component = (Component) Activator.CreateInstance(type);
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

        public bool RemoveComponent<TType>() where TType : Component
        {
            TType component = GetComponent<TType>();
            return RemoveComponent(component);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

            foreach (Component component in Components.ToList())
            {
                component.InternalInitialize(this);
            }

            State = LifecycleState.Initialized;
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
            foreach (Component component in Components)
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
            _children.Add(entity, () => { entity.Parent = this; });
        }

        public bool RemoveChild(Entity entity)
        {
            return _children.Remove(entity);
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


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // foreach (Component component in Components)
            // {
            //     component.InternalInitialize(this);
            // }

            foreach (IEntity child in Children)
            {
                ((Entity)child).Parent = this;
            }
        }
    }
}
