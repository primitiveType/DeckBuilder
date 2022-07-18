using System;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class Armor : SummerJam1Component, IAmount, ITooltip
    {
        public int Amount { get; set; }

        [OnRequestDamageReduction]
        private void OnRequestDealDamage(object sender, RequestDamageReductionEventArgs args)
        {
            if (args.Target != Entity)
            {
                return;
            }
            var blocked = Math.Min(args.Amount, Amount);
            Amount -= blocked;
            args.Reduction.Add(blocked);
        }

        [OnBattleEnded]
        [OnAttackPhaseEnded]
        private void OnBattleEnded()
        {
            Amount = 0;
        }

        public string Tooltip => Tooltips.ARMOR_TOOLTIP;
    }
}
