using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Api;
using Api.Extensions;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;
using SummerJam1.Piles;
using SummerJam1.Rules;
using SummerJam1.Units;
using Component = Api.Component;
using Random = Api.Random;

namespace SummerJam1
{
    public class ShopContainer : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < 3; i++)
            {
                var card = Game.CreateRandomCard();
                    card.TrySetParent(Entity);
                    var cost = card.AddComponent<BuyableCard>();
                    cost.Cost = 60;//TODO: add rarities, base cost on that. Add OnSale component
            }
        }
    }
    public class MapChoicesContainer : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Game.PropertyChanged += GameOnPropertyChanged;
            PopulateMapChoices();
        }

        private void GameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.CurrentLevel))
            {
                PopulateMapChoices();
            }
        }

        private void PopulateMapChoices()
        {
            Entity.Children.DestroyRecursive();
            
            Context.CreateEntity<BattleChoice>(Entity);
            Context.CreateEntity<ShopChoice>(Entity);
        }
    }

    public abstract class EncounterChoice : SummerJam1Component, IDescription, IClickable, IVisual
    {
        public abstract string Description { get; }
        public abstract void Click();
    }

    public class BattleChoice : EncounterChoice
    {
        public override string Description { get; } = "A battle, with a booster pack as reward.";

        public override void Click()
        {
            Game.StartBattle();
        }
    }

    public class ShopChoice : EncounterChoice
    {
        public override string Description { get; } = "Shop where you can cash in treasures and spend gold.";

        public override void Click()
        {
            Game.StartShop(); 
        }
    }

    public class Game : SummerJam1Component
    {
        public DeckPile Deck { get; private set; }
        public CardPrizePile PrizePile { get; private set; }
        public RelicPrizePile RelicPrizePile { get; private set; }
        public Pile RelicPile { get; private set; }

        public BattleContainer Battle { get; private set; }
        public ShopContainer Shop { get; private set; }
        public Player Player { get; private set; }
        // public Pile PlayerUnits { get; private set; }

        public Random Random { get; private set; }

        public CardPrefabPile PrefabDebugPileTester { get; private set; }
        public IEntity PrefabsContainer { get; private set; }
        public Pile DiscardStagingPile { get; private set; }

        public int CurrentLevel { get; private set; } = 1;

        public IEntity MapChoicesContainer { get; private set; }


        protected override void Initialize()
        {
            base.Initialize();
            Logging.Log("Game Initialized.");
            AddRules();
            Random = Entity.AddComponent<Random>();
            PrizePile = Context.CreateEntity<CardPrizePile>(Entity).WithName("PrizePile");
            RelicPrizePile = Context.CreateEntity<RelicPrizePile>(Entity).WithName("RelicPrizePile");
            RelicPile = Context.CreateEntity<RelicPile>(Entity).WithName("PlayerRelicPile");
            DiscardStagingPile = Context.CreateEntity<DiscardStagingPile>(Entity).WithName("DiscardStaging");
            // Context.CreateEntity(Entity, entity => PlayerUnits = entity.AddComponent<EncounterSlotPile>());
            Player = Context.CreateEntity(Entity, "player").GetComponent<Player>().WithName("Player");
            MapChoicesContainer =
                Context.CreateEntity<MapChoicesContainer>(Entity).WithName("MapChoicesContainer").Entity;
            //temp code
            // var unit = Context.CreateEntity(PlayerUnits.Entity, "Units/Player/Knight");


            // Logging.Log($"Unit created : {unit}");
            CreatePrefabPile();
            PopulatePlayerDeck();
            Events.OnGameStarted(new GameStartedEventArgs());
        }

        private void PopulatePlayerDeck()
        {
            //create an example deck.
            Context.CreateEntity(Entity, entity =>
            {
                Deck = entity.AddComponent<DeckPile>();
                entity.AddComponent<NameComponent>().Value = "Deck";
            });

            Dictionary<string, bool> units = new Dictionary<string, bool>();
            // foreach (var child in PlayerUnits.Entity.GetComponentsInChildren<PlayerUnit>())
            // {
            //     Logging.Log(child.Entity.GetComponent<NameComponent>().Value);
            //     units.Add(child.UnitName, true);
            // }
            foreach (StartingCard prefabsContainerChild in PrefabsContainer.GetComponentsInChildren<StartingCard>())
            {
                for (int i = 0; i < prefabsContainerChild.Amount; i++)
                {
                    Context.CreateEntity(Deck.Entity,
                        prefabsContainerChild.Entity.GetComponent<SourcePrefab>().Prefab);
                }
            }
        }

        private void CreatePrefabPile()
        {
            PrefabsContainer = Context.CreateEntity(Entity);
            PrefabsContainer.AddComponent<DefaultPile>();
            DirectoryInfo info = new(Context.PrefabsPath);
            string fullPrefabsPath = info.FullName;

            CreatePrefabForFiles(info);

            CreateCardPrefabPile();

            void CreatePrefabForFiles(DirectoryInfo directoryInfo)
            {
                List<FileInfo> files = directoryInfo.GetFiles().Where(file => file.Extension == ".json").ToList();
                foreach (FileInfo fileInfo in files)
                {
                    try
                    {
                        IEntity entity = Context.CreateEntity(PrefabsContainer,
                            fileInfo.FullName.Replace(fullPrefabsPath, "").Substring(1));
                        foreach (Component entityComponent in entity.Components)
                        {
                            entityComponent.Enabled = false;
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.LogError($"Failed to deserialize card {fileInfo.Name}. {e}");
                    }
                }

                foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                {
                    CreatePrefabForFiles(directory);
                }
            }
        }

        private void CreateCardPrefabPile()
        {
            //Create separate pile for cards, just for easy testing...
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
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                CurrentLevel++;
            }
            
            Battle.Entity.Destroy();
        }

        // ReSharper disable once UnusedMember.Local
        private List<string> GetEnemyInfos(int difficulty)
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Units", "Standard", difficulty.ToString()));
            List<string> files = info.GetFiles().Where(file => file.Extension == ".json")
                .Select(file => $"Units/Standard/{difficulty}/{file.Name}")
                .ToList();
            return files;
        }

        // ReSharper disable once UnusedMember.Local
        private List<string> GetRelicInfos()
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Relics"));
            List<string> files = info.GetFiles().Where(file => file.Extension == ".json")
                .Select(file => $"Relics/{file.Name}").ToList();
            return files;
        }

        private IEnumerable<IEntity> GetRelicPrefabs(Func<IEntity, bool> constraint = null)
        {
            return PrefabsContainer.Children
                .Where(entity => entity.HasComponent<RelicComponent>())
                .Where(entity => constraint != null && constraint.Invoke(entity));
        }

        public string GetRandomRelicSource(Func<IEntity, bool> constraint = null)
        {
            return GetRelicPrefabs(constraint).Random(Random, constraint).GetComponent<SourcePrefab>().Prefab;
        }

        private IEnumerable<IEntity> GetCardPrefabs(Func<IEntity, bool> constraint = null)
        {
            return PrefabsContainer.Children
                .Where(entity => entity.HasComponent<PlayerCard>())
                .Where(entity => constraint == null || constraint.Invoke(entity));
        }

        public string GetRandomCardSource(Func<IEntity, bool> constraint = null)
        {
            return GetCardPrefabs(constraint).Random(Random, constraint).GetComponent<SourcePrefab>().Prefab;
        }


        private void AddRules()
        {
            // Context.Root.AddComponent<MovedUnitMustHaveLessHealthThanUnitUnderneath>();
            Context.Root.AddComponent<DiscardHandOnDiscardPhase>();
            Context.Root.AddComponent<BattleEndsWhenAllEnemiesDefeated>();
            // Context.Root.AddComponent<FillSlotsOnBattleStarted>();
            Context.Root.AddComponent<DrawHandOnTurnBegin>();
            // Context.Root.AddComponent<DrawEncounterHandOnTurnBegin>();
            // Context.Root.AddComponent<DrawEncounterHandWhenEmpty>();
        }


        public void WaitForCard()
        {
            Events.OnWaitForCard(new WaitForCardEventArgs());
            Battle.BattleDeck.DrawCard();
        }

        public void EndTurn()
        {
            Events.OnTurnEnded(new TurnEndedEventArgs());
            Events.OnAttackPhaseStarted(new AttackPhaseStartedEventArgs());
            Events.OnAttackPhaseEnded(new AttackPhaseEndedEventArgs());
            // Events.OnMovementPhaseBegan(new MovementPhaseBeganEventArgs());

            // Events.OnDungeonPhaseStarted(new DungeonPhaseStartedEventArgs());
            Events.OnDiscardPhaseBegan(new DiscardPhaseBeganEventArgs());
            Events.OnDrawPhaseBegan(new DrawPhaseBeganEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }


        public void StartBattle()
        {
            Battle?.Entity.Destroy();

            Battle = Context.CreateEntity<BattleContainer>(Entity).WithName("BattleContainer");
            Battle.StartBattle();
        }


        public List<string> GetBattlePrefabs(int min, int max)
        {
            List<string> prefabs = new();
            int count = Random.SystemRandom.Next(min, max);
            for (int i = 0; i < count; i++)
            {
                prefabs.Add(
                    BattleContainer.GetRandomMonsterPrefab(1, Game.CurrentLevel, Entity.GetComponent<Random>()));
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
        public IEntity CreateRandomTreasureCard()
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Cards/Treasure"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Cards/Treasure", files[index].Name));
        }

        public IEntity CreateRandomRelic()
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Relics"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Relics", files[index].Name));
        }

        public void StartShop()
        {
            Shop?.Entity.Destroy();
            Shop = Context.CreateEntity<ShopContainer>();
            Events.OnShopStarted(new ShopStartedEventArgs());
        }

        public void EndShop()
        {
            Shop?.Entity.Destroy();
            Events.OnShopEnded(new ShopEndedEventArgs());
        }
    }
}