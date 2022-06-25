using Api;

namespace SummerJam1.Cards
{
    public class DrawCardForEachFriendly : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            for (int i = 0; i < Game.Battle.GetFriendlies().Count; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }

            return true;
        }
    }
}