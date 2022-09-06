﻿using System;
using System.Collections.Generic;
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

        public int CurrentLevel { get; private set; } = 1;

        // public List<Pile> Dungeons { get; private set; } = new List<Pile>();

        public IEntity Dungeons { get; private set; }

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
            PopulatePlayerDeck();
            PopulateDungeons();
            Events.OnGameStarted(new GameStartedEventArgs());
        }

        private void PopulatePlayerDeck()
        {
            //create an example deck.
            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            for (int i = 0; i < 3; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/Strike.json");
            }

            for (int i = 0; i < 3; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/Block.json");
            }


            Context.CreateEntity(Deck.Entity, "Cards/Backpack.json");
            Context.CreateEntity(Deck.Entity, "Cards/Frostbite.json");
            Context.CreateEntity(Deck.Entity, "Cards/DesperateStrike.json");
            Context.CreateEntity(Deck.Entity, "Cards/DrawStrength.json");
            Context.CreateEntity(Deck.Entity, "Cards/Preparation.json");
        }

        private void CreatePrefabPile()
        {
            Context.CreateEntity(Context.Root, entity => PrefabDebugPileTester = entity.AddComponent<CardPrefabPile>());

            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Cards"));
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

        [OnBattleEnded]
        public void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                CurrentLevel++;
            }
        }

        private List<string> GetEnemyInfos(int difficulty)
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Units", "Standard", difficulty.ToString()));
            List<string> files = info.GetFiles().Where(file => file.Extension == ".json").Select(file => $"Units/Standard/{difficulty}/{file.Name}")
                .ToList();
            return files;
        }

        private List<string> GetRelicInfos()
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Relics"));
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
            Context.Root.AddComponent<DiscardHandOnDiscardPhase>();
            Context.Root.AddComponent<FillSlotsOnBattleStarted>();
            Context.Root.AddComponent<DrawHandOnTurnBegin>();
            // Context.Root.AddComponent<DrawEncounterHandOnTurnBegin>();
            // Context.Root.AddComponent<DrawEncounterHandWhenEmpty>();
        }


        public void EndTurn()
        {
            Events.OnTurnEnded(new TurnEndedEventArgs());
            Events.OnAttackPhaseStarted(new AttackPhaseStartedEventArgs());
            Events.OnAttackPhaseEnded(new AttackPhaseEndedEventArgs());
            Events.OnMovementPhaseBegan(new MovementPhaseBeganEventArgs());

            // Events.OnDungeonPhaseStarted(new DungeonPhaseStartedEventArgs());
            Events.OnDiscardPhaseBegan(new DiscardPhaseBeganEventArgs());
            Events.OnDrawPhaseBegan(new DrawPhaseBeganEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }


        public void PopulateDungeons()
        {
            if (Dungeons == null)
            {
                Context.CreateEntity(Entity,
                    entity =>
                    {
                        Dungeons = entity;
                        Dungeons.AddComponent<DungeonParent>();
                    });
            }

            int numDungeons = 5;
            int minCards = 10;
            int maxCards = 20;

            for (int i = 0; i < numDungeons; i++)
            {
                int i1 = i;
                Context.CreateEntity(Dungeons, delegate(IEntity entity)
                {
                    DungeonPile pile = i1 switch
                    {
                        0 => entity.AddComponent<ExterminationDungeonPile>(),
                        1 => entity.AddComponent<BountyDungeonPile>(),
                        _ => entity.AddComponent<ArtifactHuntDungeonPile>()
                    };

                    pile.Difficulty = i1;
                    if (i1 % 2 == 0)
                    {
                        entity.AddComponent<CardReward>();
                        maxCards = 14;
                        minCards = 10;
                    }
                    else
                    {
                        entity.AddComponent<RelicReward>();

                        maxCards = 20;
                        minCards = 13;
                    }
                });
            }
        }

        public void StartBattle(DungeonPile pile)
        {
            Battle?.Entity.Destroy();

            Context.CreateEntity(Entity, entity =>
            {
                Battle = entity.AddComponent<BattleContainer>();
                entity.AddComponent<HandleMovementPhase>();
                entity.AddComponent<HandleAttackPhase>();
            });


            Battle.StartBattle(pile);
        }


        public List<string> GetBattlePrefabs(int min, int max)
        {
            List<string> prefabs = new();
            int count = Random.SystemRandom.Next(min, max);
            for (int i = 0; i < count; i++)
            {
                prefabs.Add(BattleContainer.GetRandomMonsterPrefab(1, Game.CurrentLevel, Entity.GetComponent<Random>()));
            }

            return prefabs;
        }


        public IEntity CreateRandomCard()
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Cards"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Cards", files[index].Name));
        }

        public IEntity CreateRandomRelic()
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Relics"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Relics", files[index].Name));
        }
    }
}
