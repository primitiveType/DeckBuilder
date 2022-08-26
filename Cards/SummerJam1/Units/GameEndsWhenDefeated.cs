namespace SummerJam1.Units
{
    public class GameEndsWhenDefeated : SummerJam1Component
    {
        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                Events.OnGameEnded(new GameEndedEventArgs(true));
            }
        }
    }
}
