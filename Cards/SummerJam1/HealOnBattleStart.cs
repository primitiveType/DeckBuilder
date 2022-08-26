namespace SummerJam1
{
    public class HealOnBattleStart : SummerJam1Component
    {
        [OnBattleStarted]
        private void OnBattlStarted(object sender, BattleStartedEventArgs args)
        {
            Health health = Entity.GetComponent<Health>();
            health.TryHeal(health.Max, Entity);
        }
    }
}