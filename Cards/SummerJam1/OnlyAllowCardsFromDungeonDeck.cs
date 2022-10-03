using Api;

namespace SummerJam1
{
    public class OnlyAllowCardsFromDungeonDeck : SummerJam1Component, IParentConstraint
    {
        public bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public bool AcceptsChild(IEntity child)
        {
            return child.Parent == Game.Battle.EncounterDrawPile.Entity;
        }
    }
}