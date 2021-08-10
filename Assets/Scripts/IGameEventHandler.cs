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

public delegate void CardPlayedEvent(object sender, CardPlayedEventArgs args);

public class CardPlayedEventArgs
{
    public CardPlayedEventArgs(int cardId)
    {
        CardId = cardId;
    }

    public int CardId { get; }
}