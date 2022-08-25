using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Rules;
using Random = Api.Random;

namespace SummerJam1
{
    public class Game : SummerJam1Component
    {
        public DeckPile Deck { get; private set; }
        public CardPrizePile PrizePile { get; private set; }
        public RelicPrizePile RelicPrizePile { get; private set; }
        public Pile RelicPile { get; private set; }

        public BattleContainer Battle { get; private set; }
        public Player Player { get; private set; }

        public Random Random { get; private set; }

        public Pile PrefabDebugPileTester { get; private set; }
        public Pile DiscardStagingPile { get; private set; }
        


        protected override void Initialize()
        {
            base.Initialize();
            Logging.Log("Game Initialized.");
            AddRules();
            Random = Entity.AddComponent<Random>();
            Context.CreateEntity(Entity, entity => PrizePile = entity.AddComponent<CardPrizePile>());
            Context.CreateEntity(Entity, entity => RelicPrizePile = entity.AddComponent<RelicPrizePile>());
            Context.CreateEntity(Entity, entity => RelicPile = entity.AddComponent<RelicPile>());
            Context.CreateEntity(Entity, entity => DiscardStagingPile = entity.AddComponent<DiscardStagingPile>());
            Player = Context.CreateEntity(Entity, "player").GetComponent<Player>();

            CreatePrefabPile();

            //create an example deck.
            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            for (int i = 0; i < 3; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/Pulse.json");
            }

            for (int i = 0; i < 3; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/NewBlockCard.json");
            }

            
            Context.CreateEntity(Deck.Entity, "Cards/Backpack.json");
            Context.CreateEntity(Deck.Entity, "Cards/ConcussivePulse.json");
            Context.CreateEntity(Deck.Entity, "Cards/DesperateStrike.json");
            Context.CreateEntity(Deck.Entity, "Cards/DrawStrength.json");
            Context.CreateEntity(Deck.Entity, "Cards/Shivers.json");

            
            Events.OnGameStarted(new GameStartedEventArgs());
        }

        private void CreatePrefabPile()
        {
            Context.CreateEntity(Context.Root, entity => PrefabDebugPileTester = entity.AddComponent<CardPrefabPile>());

            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Cards"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();
            foreach (FileInfo fileInfo in files)
            {
                try
                {
                    Context.CreateEntity(PrefabDebugPileTester.Entity, Path.Combine("Cards", fileInfo.Name));
                }
                catch (Exception e)
                {
                    Logging.LogError($"Failed to deserialize card {fileInfo.Name}. {e}");
                }
            }
        }


        private List<string> GetEnemyInfos(int difficulty)
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Units", "Standard", difficulty.ToString()));
            List<string> files = info.GetFiles().Where(file => file.Extension == ".json").Select(file => $"Units/Standard/{difficulty}/{file.Name}")
                .ToList();
            return files;
        }

        private List<string> GetRelicInfos()
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Relics"));
            List<string> files = info.GetFiles().Where(file => file.Extension == ".json").Select(file => $"Relics/{file.Name}").ToList();
            return files;
        }

        private void CreateEnemyInCell(IEntity customCellEntity, string prefab)
        {
            IEntity enemyEntity = Context.CreateEntity(customCellEntity, prefab);
        }

        private void CreateShrineInCell(IEntity customCellEntity)
        {
            IEntity enemyEntity = Context.CreateEntity(customCellEntity, entity => { entity.AddComponent<Position>(); });

            enemyEntity.AddComponent<ShrineEncounter>();
            enemyEntity.AddComponent<VisualComponent>().AssetName = "bloodAltar";
        }

        private void CreateRelicInCell(IEntity customCellEntity, string prefab)
        {
            IEntity enemyEntity = Context.CreateEntity(customCellEntity, entity => { entity.AddComponent<Position>(); });

            enemyEntity.AddComponent<RelicEncounter>().Prefab = prefab;
            enemyEntity.AddComponent<VisualComponent>().AssetName = "relic";
        }

        private void AddRules()
        {
            Context.Root.AddComponent<DiscardHandOnTurnEnd>();
            // Context.Root.AddComponent<FillSlotsOnTurnEnd>();
            Context.Root.AddComponent<DrawHandOnTurnBegin>();
            Context.Root.AddComponent<DrawEncounterHandOnTurnBegin>();
        }



        public void EndTurn()
        {
            Events.OnTurnEnded(new TurnEndedEventArgs());
            Events.OnAttackPhaseStarted(new AttackPhaseStartedEventArgs());
            Events.OnAttackPhaseEnded(new AttackPhaseEndedEventArgs());
            // Events.OnDungeonPhaseStarted(new DungeonPhaseStartedEventArgs());
            Events.OnDrawPhaseBegan(new DrawPhaseBeganEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }


        public void StartBattle()
        {
            if (Battle != null)
            {
                Battle.Entity.Destroy();
            }

            Context.CreateEntity(Entity, entity => { Battle = entity.AddComponent<BattleContainer>(); });
            Battle.StartBattle();
        }
        


        public IEntity CreateRandomCard()
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Cards"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Cards", files[index].Name));
        }

        public IEntity CreateRandomRelic()
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Relics"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Relics", files[index].Name));
        }
    }


    public class ShrineEncounter : Encounter
    {
        protected override void PlayerEnteredCell()
        {
            Events.OnRequestRemoveCard(new RequestRemoveCardEventArgs());
            Entity.Destroy();
        }
    }

    public class RelicEncounter : Encounter
    {
        public string Prefab { get; set; }

        protected override void PlayerEnteredCell()
        {
            Context.CreateEntity(Game.RelicPrizePile.Entity, Prefab);
            Entity.Destroy();
        }
    }


    public abstract class Encounter : SummerJam1Component, IVisual
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.Parent.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity item in e.NewItems)
                {
                    if (item.GetComponent<Player>() != null)
                    {
                        PlayerEnteredCell();
                    }
                }
            }
        }

        protected abstract void PlayerEnteredCell();


        public override void Terminate()
        {
            base.Terminate();
            Entity.Parent.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }
}
