using Api;

namespace SummerJam1.Cards.Effects
{
    public class DrawCards : SummerJam1Component, IEffect, IAmount
    {
        public int Amount { get; set; } = 2;

        public bool DoEffect(IEntity target)
        {
            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }

            return true;
        }

    }
}
