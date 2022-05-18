namespace Api;

public enum EventAttachmentLifetime
{
    Battle,
    Permanent
}

public class EventAttribute : Attribute
{
    public readonly EventAttachmentLifetime Lifetime;

    public EventAttribute(EventAttachmentLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}