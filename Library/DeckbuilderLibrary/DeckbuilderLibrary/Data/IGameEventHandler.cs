using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data
{
    public interface IGameEventHandler
    {
        event CardMovedEvent CardMoved;

        // Todo(Arthur, Snapper): CardPlayedEvent is not being used when the card is played. Rather, it is used to resolve a card that has been already played. We either need to rename this function or make a new one to more accurately describe its usage. 
    event CardPlayedEvent CardPlayed;

        event CardCreatedEvent CardCreated;
        event DamageDealt DamageDealt;
        event RequestDamageAmountEvent DamageAmountRequested;
        event ActorDiedEvent ActorDied;
        event BattleEndedEvent BattleEnded;
        event TurnEndedEvent TurnEnded;
        event IntentChangedEvent IntentChanged;
        void InvokeTurnEnded(object sender, TurnEndedEventArgs args);
        void InvokeIntentChanged(object sender, IntentChangedEventArgs args);
    }
}
