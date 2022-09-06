using System.Linq;

namespace SummerJam1.Rules
{
    public class HandleAttackPhase : SummerJam1Component
    {
        [OnAttackPhaseStarted]
        private void OnAttackPhaseStarted(object sender, AttackPhaseStartedEventArgs args)
        {
            for (int i = 0; i < BattleContainer.NumEncounterSlotsPerFloor; i++)
            {
                Intent intent = Game.Battle.EncounterSlots[i].Entity.Children.FirstOrDefault()?.GetComponent<Intent>();
                intent?.DoIntent();
            }
        }
    }
}