using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace Api
{
   
    public abstract class Component : IComponent
    {
        protected Context Context => Parent.Context;
        private List<EventHandle> EventHandles { get; } = new List<EventHandle>();
        [JsonIgnore] public IEntity Parent { get; private set; }
        private bool Initialized { get; set; }

        public void InternalInitialize(IEntity parent)
        {
            if (Initialized)
            {
                return;
            }

            Initialized = true;
            Parent = parent;
            //get attributes on each component.
            var type = GetType();
            foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Public |
                                                          BindingFlags.Instance))
            {
                foreach (EventsBaseAttribute attribute in method.GetCustomAttributes<EventsBaseAttribute>())
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