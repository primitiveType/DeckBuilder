using System;
using System.Collections.Generic;
using System.Linq;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    [Serializable]
    internal class Battle : GameEntity, IBattle
    {
        internal IInternalBattleEventHandler Events { get; } = new BattleEventHandler();

        [JsonProperty] public PlayerActor Player { get; private set; }
        [JsonProperty] public List<Actor> Enemies { get; private set; }
        [JsonProperty] public IBattleDeck Deck { get; private set; }
        [JsonProperty] public List<GameEntity> Rules { get; private set; } = new List<GameEntity>();

        [JsonIgnore] private Dictionary<int, IGameEntity> EntitiesById = new Dictionary<int, IGameEntity>();

        protected override void Initialize()
        {
            base.Initialize();
            Events.ActorDied += EventsOnActorDied;
        }

        private void EventsOnActorDied(object sender, ActorDiedEventArgs args)
        {
            if (args.Actor == Player || Enemies.TrueForAll(e => e.Health <= 0))
            {
                //Todo, it is probably possible for the player and final enemy to die at the same time, which would technically result in a victory right now.
                //It should probably be a loss though.
                ((IInternalBattleEventHandler)Events).InvokeBattleEnded(this,
                    new BattleEndedEventArgs(Player.Health > 0));
            }
        }

        public void SetPlayer(PlayerActor player)
        {
            if (Player != null)
            {
                throw new NotSupportedException("It is not allowed to set the player on a battle more than once!");
            }

            Player = player;
            AddEntity(player);
        }

        public IActor GetActorById(int id)
        {
            try
            {
                return AllActors().First(actor => actor.Id == id);
            }
            catch (Exception e)
            {
                Console.Error.Write($"Caught exception trying to find actor with ID : {id}. {e.Message}");
                throw e;
            }
        }

        private IEnumerable<IActor> AllActors()
        {
            yield return Player;
            foreach (var actor in Enemies)
            {
                yield return actor;
            }
        }

        public void AddEnemy(Actor enemy)
        {
            if (Enemies == null)
            {
                Enemies = new List<Actor>();
            }

            if (Enemies.Contains(enemy))
            {
                throw new NotSupportedException("It is not allowed to set the same enemy on a battle more than once!");
            }

            Enemies.Add(enemy);
            AddEntity(enemy);
        }

        public void AddRule(GameEntity rule)
        {
            if (Rules == null)
            {
                Rules = new List<GameEntity>();
            }

            if (Rules.Contains(rule))
            {
                throw new NotSupportedException(
                    "It is not allowed to set the same rule object on a battle more than once!");
            }

            Rules.Add(rule);
            AddEntity(rule);
        }

        public void AddEntity(IGameEntity entity)
        {
            EntitiesById.Add(entity.Id, entity);
        }

        internal void SetDeck(IBattleDeck battleDeck)
        {
            if (Deck != null)
            {
                throw new NotSupportedException("It is not allowed to set the deck on a battle more than once!");
            }

            Deck = battleDeck;
            AddEntity(battleDeck);
            foreach (var card in Deck.AllCards())
            {
                AddEntity(card);
            }
        }

        public void TrySendToPile(int cardId, PileType pileType)
        {
            if (!EntitiesById.TryGetValue(cardId, out IGameEntity entity))
            {
                throw new ArgumentException($"Failed to find card with id {cardId}!");
            }

            if (entity is Card card)
            {
                Deck.TrySendToPile(card, pileType);
            }
            else
            {
                throw new ArgumentException($"Tried to move entity with id {cardId} but it was not a card!");
            }
        }
    }

    public abstract class BattleData : GameEntity
    {
        /// <summary>
        /// This method returns the list of enemies that will be added to the context.
        /// These get added before the battle begin event, and it gives an opportunity to initialize any necessary state
        /// before that event. For example, an enemy might start with vulnerable, or with half health.
        /// </summary>
        /// <returns></returns>
        public abstract List<Enemy> GetStartingEnemies();
    }

    public class BasicBattleData : BattleData
    {
        public override List<Enemy> GetStartingEnemies()
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
            return new List<Enemy> { enemy };
        }
    }
}