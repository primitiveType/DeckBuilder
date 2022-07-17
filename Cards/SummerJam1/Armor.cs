using System;
using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class Armor : SummerJam1Component, IAmount
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
        [OnTurnBegan]
        private void OnBattleEnded()
        {
            Amount = 0;
        }
    }
}
