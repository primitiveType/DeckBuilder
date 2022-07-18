using Api;
using Api.Extensions;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class DrawCards : SummerJam1Component, IEffect, IAmount, IDescription
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

        public string Description => $"Draw {Amount} Card{Amount.ToPluralitySuffix()}.";
    }
}
