using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using DeckbuilderLibrary.Data.GameEntities.Rules;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using SCGraphTheory.AdjacencyList;

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

        // [JsonIgnore] private Dictionary<int, IGameEntity> EntitiesById = new Dictionary<int, IGameEntity>();
        [JsonIgnore] public List<Card> PlayerDeck { get; } = new List<Card>();
        public IGameEvents Events { get; } = new GameEvents();

        public static IContext CurrentContext { get; set; }


        public void EndTurn()
        {
            var internalEvents = ((IInternalBattleEventHandler)Events);
            internalEvents.InvokeTurnEnded(this, new TurnEndedEventArgs());
            internalEvents.InvokeTurnStarted(this, new TurnStartedEventArgs());
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
            return ((IInternalBattleEventHandler)Events).RequestDamageAmount(sender, baseDamage,
                owner, target);
        }

        public void TryDealDamage(GameEntity source, IActor owner, IActor target, int baseDamage)
        {
            ((IInternalActor)target).TryDealDamage(source, GetDamageAmount(source, baseDamage, target, owner),
                out int totalDamageDealt, out int healthDamageDealt);
        }

        public T CreateEntity<T>() where T : GameEntity, new()
        {
            T entity = CreateEntityNoInitialize<T>();
            InitializeEntity(entity);
            return entity;
        }

        public ActorNode CreateNodeEntity()
        {
            var node = CreateEntityNoInitialize<ActorNode>();
            throw new NotImplementedException();
            return null;
        }

        private T CreateEntityNoInitialize<T>() where T : GameEntity, new()
        {
            T entity = new T
            {
                Id = GetNextEntityId()
            };

            return entity;
        }

        public T CreateResource<T>(Actor owner, int Amount) where T : Resource<T>, IResource, new()
        {
            T entity = CreateEntityNoInitialize<T>();
            entity.Owner = owner;
            ((IInternalResource)entity).Amount = Amount;
            InitializeEntity(entity);
            return entity;
        }

        public void TrySendToPile(int cardId, PileType pileType)
        {
            CurrentBattle.TrySendToPile(cardId, pileType);
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
            ((IInternalGameEvents)Events).SetBattle((Battle)CurrentBattle);
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
                CurrentBattle.AddEntity(internalGameEntity);
            }

            ((IInternalGameContext)this).ToInitialize.Clear();
        }

        private void InitializeEntity(IGameEntity entity)
        {
            IInternalGameEntity internalGameEntity = (IInternalGameEntity)entity;
            internalGameEntity.SetContext(this);
            // CurrentBattle.AddEntity(entity);
            internalGameEntity.InternalInitialize();
        }

        public T CopyCard<T>(T card) where T : Card
        {
            CurrentContext = this;
            string contextStr = JsonConvert.SerializeObject(card, m_JsonSerializerCloneSettings);
            T copy = JsonConvert.DeserializeObject<T>(contextStr, m_JsonSerializerCloneSettings);
            InitializeNewObjects();
            return copy;
        }

        private T CopyDeck<T>(T deck) where T : List<Card>
        {
            CurrentContext = this;
            string deckStr = JsonConvert.SerializeObject(deck, m_JsonSerializerSettings);
            T copy = JsonConvert.DeserializeObject<T>(deckStr, m_JsonSerializerSettings);
            InitializeNewObjects();
            return copy;
        }


        public int GetDrawAmount(object sender, int baseDraw, IActor target, IActor owner)
        {
            throw new NotImplementedException();
        }


        public IBattleDeck CreateDeck()
        {
            return CreateEntity<BattleDeck>();
        }

        public IPile CreatePile()
        {
            return CreateEntity<Pile>();
        }


        public T CreateIntent<T>(Actor owner) where T : Intent, new()
        {
            var intent = CreateEntityNoInitialize<T>();
            intent.OwnerId = owner.Id;
            InitializeEntity(intent);
            return intent;
        }

        public T CreateActor<T>(int health, int armor) where T : Actor, new()
        {
            var actor = CreateEntity<T>();
            actor.Resources.AddResource<Health>(health);
            actor.Resources.AddResource<Armor>(armor);
            return actor;
        }

       
        
        public IBattle StartBattle(PlayerActor player, BattleData data)
        {
            var battle = CreateEntityNoInitialize<Battle>();
            CurrentBattle = battle;
            ((IInternalGameEvents)Events).SetBattle(battle);
            CurrentContext = this;
            // var tempDeck = CopyDeck(PlayerDeck);

            var battleDeck = CreateEntityNoInitialize<BattleDeck>();
            InitializeEntity(battleDeck);
            foreach (Card card in PlayerDeck)
            {
                battleDeck.DrawPile.Cards.Add(card);
            }


            battle.SetDeck(battleDeck);
            battle.SetPlayer(player);
            battle.SetData(data);
                
            

            battle.AddRule(CreateEntity<ShuffleDiscardIntoDrawWhenEmpty>());
            battle.AddRule(CreateEntity<DiscardHandAtEndOfTurn>());

            InitializeEntity(battle);
            
            //Player goes first. start the turn.
            var internalEvents = (IInternalBattleEventHandler)Events;
            ((GameEvents)Events).InvokeBattleStarted(this, new BattleStartedArgs());
            internalEvents.InvokeTurnStarted(this, new TurnStartedEventArgs());

            return battle;
        }

    }
}