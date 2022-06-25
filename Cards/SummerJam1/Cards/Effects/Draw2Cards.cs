using Api;

namespace SummerJam1.Cards
{
    public class Draw2Cards : SummerJam1Component, IEffect
    {
        private int NumToDraw { get; } = 2;

        public bool DoEffect(IEntity target)
        {
            for (int i = 0; i < NumToDraw; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }

            return true;
        }

    }
}
