using Api;
using CardsAndPiles;

namespace SummerJam1.Units
{
    public abstract class Unit : SummerJam1Component, IVisual
    {
        protected override void Initialize()
        {
            base.Initialize();
            CreateIntents();
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
        
        //TODO: move this into a different component, probably

        [OnBeatMoved]
        private void OnBeatMoved(object sender, BeatMovedEventArgs args)
        {
            if (args.DidOverload)
            {
                CreateIntents();
            }
        }

        private void CreateIntents()
        { //all previous intents should have removed themselves already.
            //lets add new ones.
            for (int i = 0; i < 3; i++)
            {
                //this is arbitrary right now. Need to somehow make it data driven...
                DamageIntent intent = Entity.AddComponent<DamageIntent>();
                intent.TargetBeat = 3 * (i + 1); //damn shes fine
            }
        }
    }
}
