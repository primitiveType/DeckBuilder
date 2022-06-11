using Api;
using SummerJam1.Units;

namespace SummerJam1
{
    public class EnemyUnitSlotConstraint : Component, IParentConstraint
    {
        public bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<EnemyUnitSlot>() != null;
        }

        public bool AcceptsChild(IEntity child)
        {
            return false;
        }
    }
}