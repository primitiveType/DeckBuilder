using System;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Armor : SummerJam1Component, IAmount
    {
        public int Amount { get; set; }

        [OnRequestDamageReduction]
        private void OnRequestDealDamage(object sender, RequestDamageReductionEventArgs args)
        {
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

    public class HealOnBattleEnd : SummerJam1Component
    {
        [OnBattleEnded]
        private void OnBattlEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                var health = Entity.GetComponent<Health>();
                health.TryHeal(health.Max, Entity);
            }
        }
    }
}
