public class CardCreatedEventArgs
{
    public CardCreatedEventArgs(int cardId)
    {
        CardId = cardId;
    }

    public int CardId { get; }
}