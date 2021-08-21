using System.Runtime.Serialization;
using Data;

public interface IGameEventHandler
{
    event CardMovedEvent CardMoved;

    event CardPlayedEvent CardPlayed;

    event CardCreatedEvent CardCreated;
    event DamageDealt DamageDealt;
    event RequestDamageAmountEvent RequestDamageAmount;
}

public interface IInternalGameEventHandler : IGameEventHandler
{
    int RequestDamage(object sender, int baseDamage, IGameEntity target);
    void InvokeCardPlayed(object sender, CardPlayedEventArgs args);
    void InvokeCardMoved(object sender, CardMovedEventArgs args);
    void InvokeDamageDealt(object sender, DamageDealtArgs args);
    void InvokeCardCreated(object sender, CardCreatedEventArgs args);
}