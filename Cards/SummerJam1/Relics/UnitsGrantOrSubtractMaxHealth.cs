using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Units;

namespace SummerJam1.Relics
{
    public class UnitsGrantOrSubtractMaxHealth : SummerJam1Component
    {
        [OnBattleEnded]
        private void OnBattleEnded()
        {
            var survivors = Game.Battle.GetFriendlies().Count;
            var playerHealth = Game.Player.Entity.GetComponent<Health>();
            playerHealth.SetMax(playerHealth.Max + (3 * survivors));
        }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity.GetComponentInParent<FriendlyUnitSlot>() != null) 
            {
                var playerHealth = Game.Player.Entity.GetComponent<Health>();
                playerHealth.SetMax(playerHealth.Max - 1);
                
            }
        }
    }
}
