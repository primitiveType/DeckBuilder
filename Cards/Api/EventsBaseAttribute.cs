using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Api
{
    [MeansImplicitUse(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Itself | ImplicitUseTargetFlags.Default | ImplicitUseTargetFlags.WithMembers)]
    public abstract class EventsBaseAttribute : Attribute
    {
        public readonly EventAttachmentLifetime Lifetime;

        public EventsBaseAttribute(EventAttachmentLifetime lifetime = EventAttachmentLifetime.Permanent) 
        {
            Lifetime = lifetime;
        }

        public abstract IDisposable GetEventHandle(MethodInfo attached, IComponent instance, EventsBase events);

    }
}


