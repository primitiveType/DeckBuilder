using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class RelicPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return child.GetComponent<RelicComponent>() != null;
        }
    }
}