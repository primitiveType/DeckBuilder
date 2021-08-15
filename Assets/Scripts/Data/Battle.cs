using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    public class Battle : GameEntity
    {
        public Actor Player { get; }
        public List<Actor> Enemies { get; }
        public Deck Deck { get; }

        [JsonConstructor]
        public Battle(int id, Properties properties, Actor player, List<Actor> enemies, Deck deck) : base(id,
            properties)
        {
            Player = player;
            Enemies = enemies;
            Deck = deck;
        }

        public Battle(Actor player, List<Actor> enemies, Deck deck, IContext context) : base(context)
        {
            Player = player;
            Enemies = enemies;
            Deck = deck;
        }
    }
}