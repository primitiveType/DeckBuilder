using Api;
using CardsAndPiles;

namespace SummerJam1.Units
{
    public class UnitSlot : Pile
    {
        public int Order { get; set; }
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}