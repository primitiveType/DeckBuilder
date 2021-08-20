public class CardPlayedEventArgs
{
    public CardPlayedEventArgs(int cardId)
    {
        CardId = cardId;
    }

    public int CardId { get; }
}