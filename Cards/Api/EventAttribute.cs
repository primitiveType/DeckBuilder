using System;
using System.Reflection;

namespace Api
{
    public enum EventAttachmentLifetime
    {
        Battle,
        Permanent
    }

    public abstract class EventAttribute : Attribute
    {
        public readonly EventAttachmentLifetime Lifetime;

        public EventAttribute(EventAttachmentLifetime lifetime = EventAttachmentLifetime.Permanent)
        {
            Lifetime = lifetime;
        }

        public abstract EventHandle GetEventHandle(MethodInfo attached, object instance, Events events);

    }
}