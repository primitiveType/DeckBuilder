using System.Linq;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Relics
{
    public class DealDamageEveryTurn : SummerJam1Component, ITooltip, IDescription
    {
        public int PlayerDamage { get; set; }
        public int EnemyDamage { get; set; }
        public string Description => Tooltip;

        public string Tooltip
        {
            get
            {
                if (PlayerDamage > 0)
                {
                    return $"At the end of every turn, take {PlayerDamage} damage and deal {EnemyDamage}.";
                }

                return $"At the end of every turn, deal {EnemyDamage} damage to the first enemy.";
            }
        }

        [OnTurnEnded]
        private void OnTurnEnded()
        {
            if (PlayerDamage > 0)
            {
                Game.Player.Entity.GetComponent<Health>().TryDealDamage(PlayerDamage, Entity);
            }

            if (EnemyDamage > 0)
            {
                Game.Battle.GetTopEntitiesInAllSlots().FirstOrDefault()?.GetComponent<Health>().TryDealDamage(EnemyDamage, Entity);
            }
        }
    }
}
