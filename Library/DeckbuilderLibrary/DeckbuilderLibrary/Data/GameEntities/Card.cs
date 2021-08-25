using System;
using System.Collections.Generic;
using System.Linq;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    [Serializable]
    [JsonConverter(typeof(GameEntityConverter))]
    public abstract class Card : GameEntity
    {
        public abstract string Name { get; }

        public abstract string GetCardText(IGameEntity target = null);

        public abstract IReadOnlyList<IActor> GetValidTargets();

        public abstract bool RequiresTarget { get; }

        [JsonIgnore] public IActor Owner => Context.GetCurrentBattle().Player;


        public void PlayCard(IActor target)
        {
            if (target == null && RequiresTarget)
            {
                throw new ArgumentException(
                    "This card requires a target, but an attempt was made to play it without one!");
            }

            if (RequiresTarget && !GetValidTargets().Contains(target))
            {
                throw new ArgumentException("Tried to play card on invalid target!");
            }

            DoPlayCard(target);
            ((IInternalGameEventHandler)Context.Events).InvokeCardPlayed(this, new CardPlayedEventArgs(Id));
            if (IsPlayable())
            {
            
            }
            else
            {
                Console.WriteLine("Attempted to play card that was not playable!");
            }
        }

        protected abstract void DoPlayCard(IActor target);

        public abstract bool IsPlayable();

        private void Log(string log)
        {
        }
    }
}