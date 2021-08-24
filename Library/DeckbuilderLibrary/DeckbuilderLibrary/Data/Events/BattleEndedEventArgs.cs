namespace DeckbuilderLibrary.Data.Events
{
    public class BattleEndedEventArgs
    {
        public bool IsVictory { get; }

        public BattleEndedEventArgs(bool isVictory)
        {
            IsVictory = isVictory;
        }
    }
}