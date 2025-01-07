using Api;
using CardsAndPiles.Components;
using SummerJam1.Cards.Effects;

namespace SummerJam1.Cards
{
    public class Unplayable : SummerJam1Component, IDescription, IEffect
    {
        public string Description { get; } = "Unplayable";


        public bool DoEffect(IEntity target)
        {
            return false;//This doesn't actually prevent the card from being played, yet. Might be fine though.
        }
    }
}