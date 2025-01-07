using SummerJam1.Units;

namespace SummerJam1
{
    public class CardReward : Reward
    {
        public override string RewardText => "Draft a new card.";

        public override void TriggerReward()
        {
            Game.PrizePile.SetupRandomPrizePile();
            Entity.Destroy();
        }
    }
}