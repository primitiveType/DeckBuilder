using CardsAndPiles.Components;

namespace SummerJam1
{
    public class HealOnBattleEnd : SummerJam1Component
    {
        [OnBattleEnded]
        private void OnBattlEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                var health = Entity.GetComponent<Health>();
                health.TryHeal(health.Max, Entity);
            }
        }
    }
}