namespace SummerJam1.Objectives
{
    public class EndWithThreeUnits : Objective
    {
        [OnBattleEnded]
        private void OnBattleEnded()
        {
            if (Game.Battle.GetFriendlies().Count > 2)
            {
                Completed = true;
            }
        }
    }
}