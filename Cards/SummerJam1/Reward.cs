using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public abstract class Reward : SummerJam1Component, IReward
    {
        public abstract string RewardText { get; }
        public abstract void TriggerReward();

        protected override void Initialize()
        {
            base.Initialize();
        
        }

        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            Logging.Log($"Reward acquired {RewardText}.");
            TriggerReward();
        }
    }
}