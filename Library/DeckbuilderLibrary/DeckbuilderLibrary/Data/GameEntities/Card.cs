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

        public abstract string GetCardText(IGameEntity target = null);

        public abstract IReadOnlyList<Actor> GetValidTargets();

        public void PlayCard(Actor target)
        {
            DoPlayCard(target);
            ((IInternalGameEventHandler)Context.Events).InvokeCardPlayed(this, new CardPlayedEventArgs(Id)); 
        }

        protected abstract void DoPlayCard(Actor target);


        private void Log(string log)
        {
        }
    }
}