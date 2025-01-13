using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Statuses
{
    public class Regen : SummerJam1Component, IAmount, IStatusEffect
    {
        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetComponent<Health>().TryHeal(Amount, Entity);
            Amount--;
            if (Amount <= 0)
            {
                Entity.RemoveComponent(this);
            }
        }
    }
    public class Vulnerable: SummerJam1Component, IStatusEffect, ITooltip, IAmount
    {
        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Target == Entity)
            {
                args.Multiplier.Add(0.5f);
            }
        }
        public string Tooltip => $"{Tooltips.VULN_TOOLTIP} for {Amount} turns";
        public int Amount { get; set; } = 1;
    }
    
    public class Weak: SummerJam1Component, IStatusEffect, ITooltip, IAmount
    {
        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Source == Entity)
            {
                args.Multiplier.Add(-0.25f);
            }
        }
        public string Tooltip => $"{Tooltips.WEAK_TOOLTIP} for {Amount} turns";
        public int Amount { get; set; }
    }

    public class AddsComponentOnHit<T> : SummerJam1Component, IAmount, IDescription where T : Component, IAmount, new()
    {
        public int Amount { get; set; } = 1;
        

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                var status = args.Target.AddComponent<T>();
                status.Amount = Amount;
            }
        }

        public string Description => $"Apply {Amount} {typeof(T).Name}";
    }

    public class AddsVulnerableOnHit: AddsComponentOnHit<Vulnerable>, ITooltip
    {
        public string Tooltip => Tooltips.VULN_TOOLTIP;
    }

    public class AddsWeakOnHit : AddsComponentOnHit<Weak>
    {
        public string Tooltip => Tooltips.WEAK_TOOLTIP;
    }
   
}