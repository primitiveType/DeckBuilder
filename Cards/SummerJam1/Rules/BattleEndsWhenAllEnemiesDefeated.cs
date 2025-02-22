using System.Linq;
using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class BattleEndsWhenAllEnemiesDefeated : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            var slot = Game.Battle.MonsterSlots;

            if (slot.Entity.Children.Count == 0 ||
                (slot.Entity.Children.Count == 1 && slot.Entity.Children.First().Id == args.Entity.Id))
            {
                Events.OnBattleEnded(new BattleEndedEventArgs(true));
            }
        }
    }
}