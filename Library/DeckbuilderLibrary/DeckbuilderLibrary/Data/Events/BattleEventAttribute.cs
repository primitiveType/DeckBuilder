using System;

namespace DeckbuilderLibrary.Data.Events
{
    public enum EntityScope
    {
        Battle,
        Campaign
    }

    public class BattleEntityAttribute : Attribute
    {
        
    }
    public class BattleEventAttribute : Attribute
    {
        public readonly EntityScope Scope;

        public BattleEventAttribute(EntityScope scope = EntityScope.Battle)
        {
            Scope = scope;
        }
    }
}