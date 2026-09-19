using Api;

namespace SummerJam1.Cards.Effects
{
    public class DrawNewHandCard : SummerJam1Component, IEffect
    {
        public TargetingType Targeting { get; } = TargetingType.Unit;
        public bool DoEffect(IEntity target)
        {
            while (Game.Battle.Hand.Discard())
            {
                //do nothing.
            }

            for (int i = 0; i < 5; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }

            return true;
        }
    }
}