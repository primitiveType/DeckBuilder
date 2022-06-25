using Api;

namespace SummerJam1.Cards
{
    public class GainEnergy : SummerJam1Component, IEffect
    {
        public int Amount { get; set; }

        public bool DoEffect(IEntity target)
        {
            Game.Player.CurrentEnergy += Amount;
            return true;
        }
    }
}