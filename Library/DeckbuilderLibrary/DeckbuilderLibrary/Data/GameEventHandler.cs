public class GameEventHandler : IGameEventHandler
{
    public event CardMovedEvent CardMoved;

    public void InvokeCardMoved(object sender, CardMovedEventArgs args)
    {
        CardMoved?.Invoke(sender, args);
    }

    public event CardPlayedEvent CardPlayed;

    public void InvokeCardPlayed(object sender, CardPlayedEventArgs args)
    {
        CardPlayed?.Invoke(sender, args);
    }

    public event CardCreatedEvent CardCreated;

    public void InvokeCardCreated(object sender, CardCreatedEventArgs args)
    {
        CardCreated?.Invoke(sender, args);
    }

    public event DamageDealt DamageDealt;

    public void InvokeDamageDealt(object sender, DamageDealtArgs args)
    {
        DamageDealt?.Invoke(sender, args);
    }
}