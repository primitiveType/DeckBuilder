using Api;

namespace SummerJam1.Cards
{
    public class DrawNewHandCard : SummerJam1Card
    {
        protected override bool PlayCard(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

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