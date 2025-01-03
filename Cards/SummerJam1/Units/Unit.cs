using System;
using Api;
using CardsAndPiles;
using Newtonsoft.Json;

namespace SummerJam1.Units
{
    public class DiesWhenAtZeroHealth : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Entity.TrySetParent(null);
            }
        }
    }
    public abstract class Unit : SummerJam1Component, IVisual
    {
        protected override void Initialize()
        {
            base.Initialize();
            ((SummerJam1Events)Context.Events).OnUnitCreated(new UnitCreatedEventArgs(Entity));
        }
    }

    public class PlayerUnit : Unit
    {
        [JsonProperty]
        public string UnitName { get; private set; }

        public PlayerUnit()
        {
            Logging.Log("Created player unit.");
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
    }

    public class CharacterClass : SummerJam1Component
    {
        
    }
}