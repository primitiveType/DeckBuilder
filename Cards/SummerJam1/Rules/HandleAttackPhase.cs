using System.Linq;

namespace SummerJam1.Rules
{
    public class HandleAttackPhase : SummerJam1Component
    {
        [OnAttackPhaseStarted]
        private void OnAttackPhaseStarted(object sender, AttackPhaseStartedEventArgs args)
        {
            for (int i = 0; i < BattleContainer.NUM_ENCOUNTER_SLOTS_PER_FLOOR; i++)
            {
                Intent intent = Game.Battle.EncounterSlots[i].Entity.Children.LastOrDefault()?.GetComponent<Intent>();
                intent?.DoIntent();
            }
        }
    }
}