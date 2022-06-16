using System;
using Api;
using SummerJam1.Units;

namespace SummerJam1.Cards
{
    public abstract class UnitCard : SummerJam1Card
    {
        protected override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("Initialized card.");
        }


        protected override bool PlayCard(IEntity target)
        {
            base.PlayCard(target);
            FriendlyUnitSlot slot = target?.GetComponent<FriendlyUnitSlot>();
            if (slot != null)
            {
                if (Entity.TrySetParent(target))
                {
                    Unit unit = CreateUnit();
                    unit.Entity.TrySetParent(target);
                    return true;
                }
            }

            return false;
        }

        public override bool AcceptsParent(IEntity parent)
        {
            return base.AcceptsParent(parent) && parent.GetComponentInChildren<Unit>() == null;
        }

        protected abstract Unit CreateUnit();
    }
}