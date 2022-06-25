namespace SummerJam1.Objectives
{
    public class EndWithNoUnits : Objective
    {
        [OnBattleEnded]
        private void OnBattleEnded()
        {
            if (Game.Battle.GetFriendlies().Count == 0)
            {
                Completed = true;
            }
        }
    }
}