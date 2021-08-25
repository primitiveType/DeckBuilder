using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using DeckbuilderLibrary.Data.GameEntities.Rules;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data
{
    public class GameContext : IContext, IInternalGameContext
    {
        [JsonConstructor]
        public GameContext()
        {
            CurrentContext = this;
        }

        List<IInternalGameEntity> IInternalGameContext.ToInitialize { get; } = new List<IInternalGameEntity>();

        [JsonProperty] private IBattle CurrentBattle { get; set; }
        [JsonIgnore] private Dictionary<int, IGameEntity> EntitiesById = new Dictionary<int, IGameEntity>();

        public IGameEventHandler Events { get; } = new GameEventHandler();
        public static IContext CurrentContext { get; set; }

        public void AddEntity(IGameEntity entity)
        {
            EntitiesById.Add(entity.Id, entity);
        }

        public IActor GetActorById(int id)
        {
            return AllActors().First(actor => actor.Id == id);
        }

        private IEnumerable<IActor> AllActors()
        {
            yield return CurrentBattle.Player;
            foreach (var actor in CurrentBattle.Enemies)
            {
                yield return actor;
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
                CurrentBattle.Deck.TrySendToPile(card, pileType);
            }
            else
            {
                throw new ArgumentException($"Tried to move entity with id {cardId} but it was not a card!");
            }
        }


        public void SetCurrentBattle(IBattle battle)
        {
            CurrentBattle = battle;
        }


        public IBattle GetCurrentBattle()
        {
            return CurrentBattle;
        }

        public int GetPlayerHealth()
        {
            return CurrentBattle.Player.Health;
        }

        public IReadOnlyList<IActor> GetEnemies()
        {
            return CurrentBattle.Enemies;
        }

        public IReadOnlyList<int> GetEnemyIds()
        {
            return CurrentBattle.Enemies.Select(enemy => enemy.Id).ToList();
        }


        [JsonProperty] private int m_NextId { get; set; }

        private int GetNextEntityId()
        {
            return m_NextId++;
        }

        public int GetDamageAmount(object sender, int baseDamage, IActor target, IActor owner)
        {
            return ((IInternalGameEventHandler)Events).RequestDamageAmount(sender, baseDamage, owner, target);
        }

        public void TryDealDamage(GameEntity source, IActor owner, IActor target, int baseDamage)
        {
            ((IInternalActor)target).TryDealDamage(source, GetDamageAmount(source, baseDamage, target, owner),
                out int totalDamageDealt, out int healthDamageDealt);
        }

        public T CreateEntity<T>() where T : GameEntity, new()
        {
            T entity = CreateEntityNoInitialize<T>();
            ((IInternalGameEntity)entity).InternalInitialize();
            return entity;
        }

        private T CreateEntityNoInitialize<T>() where T : GameEntity, new()
        {
            T entity = new T
            {
                Id = GetNextEntityId()
            };
            entity.SetContext(this);
            EntitiesById.Add(entity.Id, entity);
            return entity;
        }

        public T CreateResource<T>(Actor owner, int Amount) where T : Resource<T>, IResource, new()
        {
            T entity = CreateEntityNoInitialize<T>();
            entity.Owner = owner;
            ((IInternalResource)entity).Amount = Amount;
            ((IInternalGameEntity)entity).InternalInitialize();
            return entity;
        }

        readonly JsonSerializerSettings m_JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver(),
            Converters = new List<JsonConverter>
            {
                new GameEntityConverter()
            }
        };

        readonly JsonSerializerSettings m_JsonSerializerCloneSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver(),
            Converters = new List<JsonConverter>
            {
                new GameEntityCloneConverter()
            }
        };

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitializeNewObjects();
        }

        private void InitializeNewObjects()
        {
            for (var i = 0; i < ((IInternalGameContext)this).ToInitialize.Count; i++)
            {
                IInternalGameEntity internalGameEntity = (((IInternalGameContext)this).ToInitialize[i]);
                if (internalGameEntity.Id < 0)
                {
                    ((GameEntity)internalGameEntity).Id = GetNextEntityId();
                }

                internalGameEntity.InternalInitialize();
                AddEntity(internalGameEntity);
            }

            ((IInternalGameContext)this).ToInitialize.Clear();
        }

        public T CopyCard<T>(T card) where T : Card
        {
            CurrentContext = this;
            string contextStr = JsonConvert.SerializeObject(card, m_JsonSerializerCloneSettings);
            T copy = JsonConvert.DeserializeObject<T>(contextStr, m_JsonSerializerCloneSettings);
            InitializeNewObjects();
            return copy;
        }

        public int GetBlockAmount(object sender, int baseDamage, IActor target, IActor owner)
        {
            throw new NotImplementedException();
        }

        public int GetDrawAmount(object sender, int baseDraw, IActor target, IActor owner)
        {
            throw new NotImplementedException();
        }

        public int GetVulnerableAmount(object sender, int baseDamage, IActor target, IActor owner)
        {
            throw new NotImplementedException();
        }

        public void TryApplyBlock(GameEntity source, IActor owner, IActor target, int baseBlock)
        {
            throw new NotImplementedException();
        }

        public void TryApplyVulnerable(GameEntity source, IActor owner, IActor target, int baseVulnerable)
        {
            throw new NotImplementedException();
        }

        public IDeck CreateDeck()
        {
            return CreateEntity<Deck>();
        }

        public IPile CreatePile()
        {
            return CreateEntity<Pile>();
        }


        public T CreateIntent<T>(Actor owner) where T : Intent, new()
        {
            var intent = CreateEntity<T>();
            intent.OwnerId = owner.Id;
            return intent;
        }

        public T CreateActor<T>(int health, int armor) where T : Actor, new()
        {
            var actor = CreateEntity<T>();
            actor.Resources.AddResource<Health>(health);
            actor.Resources.AddResource<Armor>(armor);

            return actor;
        }

        public IBattle CreateBattle(IDeck deck, PlayerActor player, List<Enemy> enemies)
        {
            var battle = CreateEntity<Battle>();
            CurrentBattle = battle;
            battle.SetDeck(deck);
            battle.SetPlayer(player);
            foreach (var enemy in enemies)
            {
                battle.AddEnemy(enemy); //might need set access instead.
            }

            battle.Rules.Add(CreateEntity<ShuffleDiscardIntoDrawWhenEmpty>());
            return battle;
        }
    }
}