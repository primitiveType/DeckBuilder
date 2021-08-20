using System.Runtime.Serialization;

public interface IGameEventHandler
{
    event CardMovedEvent CardMoved;
    void InvokeCardMoved(object sender, CardMovedEventArgs args);

    event CardPlayedEvent CardPlayed;
    void InvokeCardPlayed(object sender, CardPlayedEventArgs args);

    event CardCreatedEvent CardCreated;
    void InvokeCardCreated(object sender, CardCreatedEventArgs args);
    event DamageDealt DamageDealt;
    void InvokeDamageDealt(object sender, DamageDealtArgs args);
}