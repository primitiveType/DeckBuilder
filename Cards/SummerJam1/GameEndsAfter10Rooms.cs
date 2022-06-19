namespace SummerJam1
{
    public class GameEndsAfter10Rooms : SummerJam1Component
    {
        public int RoomsCleared { get; set; }

        [OnBattleEnded]
        public void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                RoomsCleared++;
            }

            if (RoomsCleared >= 10)
            {
                Events.OnGameEnded(new GameEndedEventArgs(true));
            }
        }
    }
}
