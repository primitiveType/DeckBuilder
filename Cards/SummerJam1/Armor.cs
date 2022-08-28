using System;
using Api;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class Armor : SummerJam1Component, IAmount, ITooltip
    {
        public int Amount { get; set; }

        public string Tooltip => Tooltips.ARMOR_TOOLTIP;
        
        // [OnRequestDamageModifiers]
        // private void OnRequestDealDamage(object sender, RequestDamageModifiersEventArgrs argrs)
        // {
        //     if (argrs.Target != Entity)
        //     {
        //         return;
        //     }
        //     var blocked = Math.Min(argrs.Amount, Amount);
        //     Amount -= blocked;
        //     argrs.Reduction.Add(blocked);
        // }

        [OnBattleEnded]
        [OnAttackPhaseEnded]
        private void OnBattleEnded()
        {
            Amount = 0;
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
