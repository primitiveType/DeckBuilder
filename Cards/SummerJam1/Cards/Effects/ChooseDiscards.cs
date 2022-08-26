using Api;
using Api.Extensions;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class ChooseDiscards : SummerJam1Component, IAmount, IEffect, IDescription
    {
        public int Amount { get; set; }


        public string Description => $"Choose {Amount} Card{Amount.ToPluralitySuffix()} to discard.";


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