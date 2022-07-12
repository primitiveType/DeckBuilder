using Api;
using CardsAndPiles;

namespace SummerJam1.Cards.Effects
{
    public class ChooseDiscards : SummerJam1Component, IAmount, IEffect
    {
        public int Amount { get; set; }


        public bool DoEffect(IEntity target)
        {
            if (Amount >= Game.Battle.Hand.Entity.Children.Count)
            {
                while (Game.Battle.Hand.Discard())
                { //just discard whole hand.
                }
            }
            else
            {
                Events.OnChooseCardsToDiscard(new ChooseCardsToDiscardEventArgs(Amount, Entity));
            }

            return true;
        }
    }
}
