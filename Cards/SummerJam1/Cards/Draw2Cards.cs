using Api;

namespace SummerJam1.Cards
{
    public class Draw2Cards : SummerJam1Card
    {
        private int NumToDraw { get; } = 2;

        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            for (int i = 0; i < NumToDraw; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }

            return true;
        }

    }
}