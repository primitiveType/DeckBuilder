using Api;

namespace SummerJam1.Cards.Effects
{
    public class GainEnergy : SummerJam1Component, IEffect, IAmount
    {
        public int Amount { get; set; }

        public bool DoEffect(IEntity target)
        {
            Game.Player.CurrentEnergy += Amount;
            return true;
        }
    }
}
