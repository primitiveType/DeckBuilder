﻿using Api;
using CardsAndPiles;

namespace SummerJam1.Units
{
    public class PlayerUnit : Unit{}
    public abstract class Unit : SummerJam1Component, IPileItem
    {
        protected override void Initialize()
        {
            base.Initialize();
            Events.OnUnitCreated(new UnitCreatedEventArgs(Entity));
        }

        public virtual bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<UnitSlot>() != null;
        }

        public virtual bool AcceptsChild(IEntity child)
        {
            return true;
        }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Entity.TrySetParent(null);
            }
        }
    }
}