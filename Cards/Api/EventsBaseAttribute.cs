using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Api
{
    [MeansImplicitUse(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Itself | ImplicitUseTargetFlags.Default |
                      ImplicitUseTargetFlags.WithMembers)]
    public abstract class EventsBaseAttribute : Attribute
    {
        [PublicAPI]
        public EventAttachmentLifetime Lifetime { get; }
        
        protected EventsBaseAttribute(EventAttachmentLifetime lifetime = EventAttachmentLifetime.Permanent)
        {
            Lifetime = lifetime;
        }

        public abstract IDisposable GetEventHandle(MethodInfo attached, IComponent instance, EventsBase events);
    }
}
