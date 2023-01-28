using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Api
{
    [MeansImplicitUse(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Itself | ImplicitUseTargetFlags.Default |
                      ImplicitUseTargetFlags.WithMembers)]
    public abstract class EventsBaseAttribute : Attribute
    {
        private static int s_nextId = 0;

        static int GetNextId()
        {
            return s_nextId++;
        }
        
        public int Id { get; }

        [PublicAPI] public EventAttachmentLifetime Lifetime { get; }

        protected EventsBaseAttribute(EventAttachmentLifetime lifetime = EventAttachmentLifetime.Permanent)
        {
            Lifetime = lifetime;
            Id = GetNextId();
        }

        public abstract IDisposable GetEventHandle(MethodInfo attached, IComponent instance, EventsBase events);
    }
}
