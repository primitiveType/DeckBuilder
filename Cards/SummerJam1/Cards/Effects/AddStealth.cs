using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class AddStealth : SummerJam1Component, IEffect, IAmount, IDescription
    {
        public TargetingType Targeting { get; } = TargetingType.Self;
        public int Amount { get; set; }

        public string Description => $"Gain {Amount} Stealth.";

        public bool DoEffect(IEntity target)
        {
            return (target ?? Game.Player.Entity).GetOrAddComponent<Stealth>().TryUseStealth(-Amount);
        }
    }
}
