using System;
using Api;
using CardsAndPiles;

namespace SummerJam1.Units
{
    public abstract class Unit : SummerJam1Component, IVisual
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

        //TODO: move this into a different component, probably

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            CreateIntent();
        }

        private void CreateIntent()
        {
            //all previous intents should have removed themselves already.
            //lets add new ones.
            var random = Game.Random;
            int dmg = random.SystemRandom.Next(1, 5);
            var intent = Entity.AddComponent<DamageIntent>();
            intent.Amount = Math.Max(0, dmg);
        }
    }
}