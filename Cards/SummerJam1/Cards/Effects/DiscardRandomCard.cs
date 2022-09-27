using Api;

namespace SummerJam1.Cards.Effects
{
    public class DiscardRandomCard : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            Game.Battle.Hand.DiscardRandom();
            return true;
        }
    }
}