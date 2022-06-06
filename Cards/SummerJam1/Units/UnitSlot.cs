using Api;
using CardsAndPiles;

namespace SummerJam1.Units
{
    public class UnitSlot : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
    
    public class EnemyUnitSlot : UnitSlot{}
}