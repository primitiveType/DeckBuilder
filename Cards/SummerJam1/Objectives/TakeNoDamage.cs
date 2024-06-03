using CardsAndPiles;

namespace SummerJam1.Objectives
{
    public class TakeNoDamage : Objective
    {
        [OnBattleStarted]
        private void OnBattleStart()
        {
            Completed = true;
        }

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity)
            {
                Failed = true;
            }
        }
    }

    // public class HaveAUnitWithXHealth : Objective
    // {
    // }
}