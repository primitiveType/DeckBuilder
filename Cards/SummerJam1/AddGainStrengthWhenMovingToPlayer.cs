using Api;
using CardsAndPiles.Components;
using SummerJam1.Cards.Effects;
using SummerJam1.Units.Effects;

namespace SummerJam1
{
    public class AddGainStrengthWhenMovingToPlayer : SummerJam1Component, IEffect, IAmount, IDescription
    {
        public TargetingType Targeting{ get; } = TargetingType.Self;

        public int Amount { get; set; }
        public string Description => $"Until end of battle, whenever an enemy is moved, gain {Amount} temporary strength.";

        public bool DoEffect(IEntity target)
        {
            GainStrengthWhenMoving component = (target ?? Game.Player.Entity).AddComponent<GainStrengthWhenMoving>();
            component.Amount = Amount;
            component.AnyMovementCounts = true;
            component.ResetEachTurn = true;

            return true;
        }
        
    }
}
