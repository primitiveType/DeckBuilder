using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public abstract class Reward : SummerJam1Component, IReward
    {
        public abstract string RewardText { get; }
        public abstract void TriggerReward();

        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (Game.Battle == Entity.GetComponentInParent<BattleContainer>())
            {
                Logging.Log($"Reward acquired {RewardText}.");
                TriggerReward();
            }
        }
    }
}