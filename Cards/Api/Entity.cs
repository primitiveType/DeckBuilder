using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RandN;
using RandN.Rngs;

namespace Api
{
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class Entity : IEntity
    {
        public Context Context { get; private set; }
        [JsonProperty] public int Id { get; private set; } = -1;

        [JsonProperty] public IChildrenCollection<Component> Components => m_Components;
        [JsonProperty] private ChildrenCollection<Component> m_Components = new ChildrenCollection<Component>();


        public IEntity Parent { get; private set; }

        public IChildrenCollection<IEntity> Children => m_Children;
        [JsonProperty] private ChildrenCollection<IEntity> m_Children = new ChildrenCollection<IEntity>();
        private bool Initialized { get; set; }

        internal void Initialize(Context context, int id) //game context parameter?
        {
            if (Initialized)
            {
                return;
            }

            Id = id;

            Context = context;

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

        public void
            ShuffleChildren() //this is the wrong way to do this. I should probably just use an order property where it matters. 
        {
            GetComponentInParent<Random>().Rng.ShuffleInPlace(m_Children);
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

        public void SetParent(IEntity parent)
        {
            if (Parent != null)
            {
                ((Entity)Parent).RemoveChild(this);
            }

            if (parent != null)
            {
                ((Entity)parent).AddChild(this);
            }
            else
            {
                Parent = null;
            }
        }


        public T GetComponent<T>()
        {
            return GetComponents<T>().FirstOrDefault();
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

        public List<T> GetComponentsInChildren<T>()
        {
            List<T> components = new List<T>();

            DoGetComponentsInChildren(this);

            void DoGetComponentsInChildren(Entity entity)
            {
                T component = entity.GetComponent<T>();
                if (component != null)
                {
                    components.Add(component);
                }


                foreach (Entity child in entity.Children)
                {
                    DoGetComponentsInChildren(child);
                }
            }


            return components;
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
                (child as Entity).Parent = (this);
            }
        }
    }

    public interface IEntity
    {
        Context Context { get; }
        int Id { get; }
        IEntity Parent { get; }
        IChildrenCollection<IEntity> Children { get; }
        void SetParent(IEntity parent);
        T GetComponent<T>();
        List<T> GetComponents<T>();
        T GetComponentInParent<T>();
        T GetComponentInChildren<T>();
        List<T> GetComponentsInChildren<T>();
        T AddComponent<T>() where T : Component, new();
        bool RemoveComponent(Component toRemove);
    }
}