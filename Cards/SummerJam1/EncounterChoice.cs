using Api;
using CardsAndPiles.Components;
using SummerJam1.Units;

namespace SummerJam1
{
    public abstract class EncounterChoice : SummerJam1Component, IDescription, IClickable, IVisual
    {
        public abstract string Description { get; }
        public abstract void Click();
    }
}