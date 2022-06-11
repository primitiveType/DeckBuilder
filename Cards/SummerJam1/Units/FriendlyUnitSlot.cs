using Api;
using SummerJam1.Cards;

namespace SummerJam1.Units
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
}