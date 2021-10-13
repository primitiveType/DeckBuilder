using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data
{
    public interface IBattleEventHandler
    {
        event CardMovedEvent CardMoved;

        event CardPlayedEvent CardPlayed;

        event CardCreatedEvent CardCreated;
        event DamageDealt DamageDealt;
        event RequestDamageAmountEvent DamageAmountRequested;
        event ActorDiedEvent ActorDied;
        event BattleEndedEvent BattleEnded;
        event TurnEndedEvent TurnEnded;
        event TurnStartedEvent TurnStarted;
        event IntentChangedEvent IntentChanged;
        event ActorsSwappedEvent ActorsSwapped;

        event DiscoverCardsEvent DiscoverCards;
    }

    public delegate void TurnStartedEvent(object sender, TurnStartedEventArgs args); //turn number

    public class TurnStartedEventArgs
    {
    }
}