using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

    public sealed class Entity
    {
        public int Id { get; private set; }
        public HashSet<Component> Components { get; } = new HashSet<Component>();
        public Entity Parent { get; private set; }
        private List<EventHandle> Handles { get; } = new List<EventHandle>();

        public IReadOnlyList<Entity> Children => m_Children;
        private List<Entity> m_Children = new List<Entity>();

        internal void Initialize() //game context paramater?
        {
            //call generated code that does reflection to get all event attributes and subscribes to proper events
            //should also iterate components? OR should it only happen in components? depends on whether this stays sealed.

            foreach (var component in Components)
            {
                //get attributes on each component.
                var type = component.GetType();
                foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance))
                {
                    foreach (EventAttribute attribute in method.GetCustomAttributes<EventAttribute>())
                    {
                        Handles.Add(attribute.GetEventHandle(method, component, GetComponentInParent<Events>()));
                    }
                }
            }
        }

        internal void Terminate()
        {
            //dispose event handles we created from initialize.
            foreach (var handle in Handles)
            {
                handle.Dispose();
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
    }
    
    

    public class Component
    {
    }

    public class Card : Component
    {
    }

    public class Context : Component
    {
        
    }
}