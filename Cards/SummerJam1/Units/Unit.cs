using System;
using System.Security.Cryptography;
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


        [OnBattleStarted]
        private void OnBattleStarted(object sender, BattleStartedEventArgs args)
        {
            CreateIntents();
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
        {
            //all previous intents should have removed themselves already.
            //lets add new ones.
            var random = Game.Random;

            int beat = random.SystemRandom.Next(1, 5);

            //this is arbitrary right now. Need to somehow make it data driven...
            Context.CreateEntity(Entity, child =>
            {
                var intent = child.AddComponent<DamageIntent>();
                intent.TargetBeat = beat; //damn shes fine
                intent.Amount = Math.Max(0, beat);
            });
        }
    }
}