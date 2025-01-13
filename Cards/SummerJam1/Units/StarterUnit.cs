using System;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Cards;
using SummerJam1.Piles;

namespace SummerJam1.Units
{
    public class StarterUnit : Unit, IDraggable, IGrantsEnergy, IMonster
    {
        [JsonIgnore] public bool CanDrag => Entity.Parent.Children.LastOrDefault() == Entity;


        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            if (args.Target.GetComponent<EncounterSlotPile>() == null)
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
                return;
            }


            RequestMoveUnitEventArgs tryMoveArgs = new(Entity, true, args.Target);
            ((SummerJam1Events)Events).OnRequestMoveUnit(tryMoveArgs);
            if (tryMoveArgs.Blockers.Any())
            {
                args.Blockers.Add(CardBlockers.UNIT_CANT_MOVE);

                foreach (string blocker in tryMoveArgs.Blockers)
                {
                    Logging.Logger.Log($"Unable to move unit : {blocker}.");
                }
            }
        }

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            EncounterSlotPile slot = args.Target.GetComponentInSelfOrParent<EncounterSlotPile>();

            if (Entity.TrySetParent(slot.Entity))
            {
                ((SummerJam1Events)Events).OnUnitMoved(new UnitMovedEventArgs(Entity, true, slot.Entity));
            }
        }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                //should I call destroy instead? I'm not sure why this is like this.
                Entity.TrySetParent(null);
            }
        }
        
       
        
       
    }

    public class RandomIntentHandler : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            CreateIntent();
        }

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            CreateIntent();
        }

        private void CreateIntent()
        {
            //all previous intents should have removed themselves already.
            //lets add new ones.
            var intents = Entity.GetComponents<Intent>();
            
            var random = Game.Random;
            int active = random.SystemRandom.Next(0, intents.Count);

            for (int i = 0; i < intents.Count; i++)
            {
                intents[i].Enabled = i == active;
            }
        }
    }
}
