using System.Runtime.Serialization;
using Data;
using DeckbuilderLibrary.Data;

public interface IGameEventHandler
{
    event CardMovedEvent CardMoved;

    event CardPlayedEvent CardPlayed;

    event CardCreatedEvent CardCreated;
    event DamageDealt DamageDealt;
    event RequestDamageAmountEvent RequestDamageAmount;
    event ActorDiedEvent ActorDied;
    event BattleEndedEvent BattleEnded;
}

//This interface exists to hide stuff on the game event handler that we don't want to be accessible when creating content.
//For example, we don't want cards to be able to invoke events directly.
internal interface IInternalGameEventHandler : IGameEventHandler
{
    //To request damage, cards should use the game context.
    int RequestDamage(object sender, int baseDamage, IGameEntity target);
    void InvokeCardPlayed(object sender, CardPlayedEventArgs args);
    void InvokeCardMoved(object sender, CardMovedEventArgs args);
    void InvokeDamageDealt(object sender, DamageDealtArgs args);
    void InvokeCardCreated(object sender, CardCreatedEventArgs args);
    void InvokeActorDied(object sender, ActorDiedEventArgs args);
    void InvokeBattleEnded(object sender, BattleEndedEventArgs args);
}