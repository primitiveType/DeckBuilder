using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace Api
{
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}