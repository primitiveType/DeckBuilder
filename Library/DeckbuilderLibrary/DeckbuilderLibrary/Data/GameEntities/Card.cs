using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    [JsonConverter(typeof(GameEntityConverter))]
    public abstract class Card : GameEntity
    {
        public abstract string Name { get; }
        

        public abstract IReadOnlyList<Actor> GetValidTargets();

        public abstract void PlayCard(Actor target);


        private void Log(string log)
        {
        }
    }
}