using System;
using Api;

namespace SummerJam1.Units
{
    public abstract class Unit : SummerJam1Component, IVisual
    {
        protected override void Initialize()
        {
            base.Initialize();
            ((SummerJam1Events)Context.Events).OnUnitCreated(new UnitCreatedEventArgs(Entity));
        }
    }
}