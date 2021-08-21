using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DeckbuilderLibrary.Data;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    internal class Battle : GameEntity, IBattle
    {
        [JsonProperty] public IActor Player { get; private set; }
        [JsonProperty] private List<IActor> Enemies { get; set; }
        [JsonProperty] public IDeck Deck { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.ActorDied += EventsOnActorDied;
        }

        private void EventsOnActorDied(object sender, ActorDiedEventArgs args)
        {
            if (args.Actor == Player || Enemies.TrueForAll(e => e.Health <= 0))
            {
                //Todo, it is probably possible for the player and final enemy to die at the same time, which would technically result in a victory right now.
                //It should probably be a loss though.
                ((IInternalGameEventHandler)Context.Events).InvokeBattleEnded(this, new BattleEndedEventArgs(Player.Health > 0));
            }
        }

        public void SetPlayer(IActor player)
        {
            if (Player != null)
            {
                throw new NotSupportedException("It is not allowed to set the player on a battle more than once!");
            }

            Player = player;
        }

        public void AddEnemy(IActor enemy)
        {
            if (Enemies == null)
            {
                Enemies = new List<IActor>();
            }

            if (Enemies.Contains(enemy))
            {
                throw new NotSupportedException("It is not allowed to set the same enemy on a battle more than once!");
            }

            Enemies.Add(enemy);
        }

        IReadOnlyList<IActor> IBattle.Enemies => Enemies;
    }
}


public interface IBattle
{
    IReadOnlyList<IActor> Enemies { get; }
    IActor Player { get; }

    IDeck Deck { get; }
}