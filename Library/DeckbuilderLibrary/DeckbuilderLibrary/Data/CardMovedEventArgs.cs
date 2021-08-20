public struct CardMovedEventArgs
{
    public PileType PreviousPileType;
    public PileType NewPileType;
    public int MovedCard;

    public CardMovedEventArgs(int movedCard, PileType newPileType, PileType previousPileType)
    {
        MovedCard = movedCard;
        NewPileType = newPileType;
        PreviousPileType = previousPileType;
    }
}