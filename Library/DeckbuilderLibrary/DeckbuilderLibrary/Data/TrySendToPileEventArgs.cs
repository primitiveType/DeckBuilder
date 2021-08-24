public class TrySendToPileEventArgs
{
    public TrySendToPileEventArgs(int cardId, PileType pileType)
    {
        CardId = cardId;
    }

    public int CardId { get; }
}