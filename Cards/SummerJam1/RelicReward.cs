namespace SummerJam1
{
    public class RelicReward : Reward
    {
        public override string RewardText => "Draft a new relic.";

        public override void TriggerReward()
        {
            Game.RelicPrizePile.SetupPrizePile();
        }
    }
}