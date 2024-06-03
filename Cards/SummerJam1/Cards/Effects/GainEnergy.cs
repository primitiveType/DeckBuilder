using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class GainEnergy : SummerJam1Component, IEffect, IAmount, IDescription
    {
        public int Amount { get; set; }

        public string Description => $"Gain {Amount} Energy.";

        public bool DoEffect(IEntity target)
        {
            Game.Player.CurrentEnergy += Amount;
            return true;
        }
    }
}