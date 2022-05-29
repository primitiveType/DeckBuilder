using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Api
{
    [MeansImplicitUse]
    public abstract class EventAttribute : Attribute
    {
        public readonly EventAttachmentLifetime Lifetime;

        public EventAttribute(EventAttachmentLifetime lifetime = EventAttachmentLifetime.Permanent) 
        {
            Lifetime = lifetime;
        }

        public abstract EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events);

    }
}