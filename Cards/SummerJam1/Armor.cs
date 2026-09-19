using System;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;
using SummerJam1.Units;

namespace SummerJam1
{
    public class Armor : SummerJam1Component, IAmount, ITooltip
    {
        public int Amount { get; set; }

        public string Tooltip => Tooltips.ARMOR_TOOLTIP;
        

        [OnBattleEnded]
        [OnAttackPhaseEnded]
        private void OnBattleEnded()
        {
            if (Entity.HasComponent<Player>())
            {
                Amount = 0;
            }
        }

        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (Entity.HasComponent<StarterUnit>())
            {
                Amount = 0;
            }
        }

        /// <summary>
        ///     Returns remaining damage.
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public int TryDealDamage(int damage)
        {
            int amountBefore = Amount;
            Amount = Math.Max(Amount - damage, 0);
            int damageDealt = amountBefore - Amount;


            return damage - damageDealt;
        }
    }
}
