using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Units
{
    public abstract class Unit : Card
    {
        protected override void Initialize()
        {
            base.Initialize();
            ((SummerJam1Events)Context.Events).OnUnitCreated(new UnitCreatedEventArgs(Entity));
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