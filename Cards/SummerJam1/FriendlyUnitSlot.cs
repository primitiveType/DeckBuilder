using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class FriendlyUnitSlot : UnitSlot
    {
        public override bool AcceptsChild(IEntity child)
        {
            UnitCard card = child.GetComponent<UnitCard>();
            Unit unit = child.GetComponent<Unit>();
            if (card != null || unit != null)
            {
                return Entity.GetComponentInChildren<Unit>() == null;
            }

            return true;
        }
    }

    public class UnitSlot : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}