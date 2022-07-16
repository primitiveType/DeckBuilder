using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using RogueMaps;
using SummerJam1.Rules;
using SummerJam1.Units;
using Random = Api.Random;

namespace SummerJam1
{
    public class Game : SummerJam1Component
    {
        public DeckPile Deck { get; private set; }
        public SummerJam1PrizePile PrizePile { get; private set; }
        public SummerJam1RelicPrizePile RelicPrizePile { get; private set; }
        public Pile RelicPile { get; private set; }

        public BattleContainer Battle { get; private set; }
        public Player Player { get; private set; }

        private UnitSlot PlayerSlot { get; set; }

        public Random Random { get; private set; }

        public CustomMap CurrentMap { get; private set; }

        public int CurrentLevel { get; private set; }

        public Pile PrefabDebugPileTester { get; private set; }
        public Pile DiscardStagingPile { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Logging.Log("Game Initialized.");
            AddRules();
            Random = Entity.AddComponent<Random>();
            Context.CreateEntity(Entity, entity => PrizePile = entity.AddComponent<SummerJam1PrizePile>());
            Context.CreateEntity(Entity, entity => RelicPrizePile = entity.AddComponent<SummerJam1RelicPrizePile>());
            Context.CreateEntity(Entity, entity => RelicPile = entity.AddComponent<RelicPile>());
            Context.CreateEntity(Entity, entity => DiscardStagingPile = entity.AddComponent<DiscardStagingPile>());
            Context.CreateEntity(Entity, entity => { PlayerSlot = entity.AddComponent<UnitSlot>(); });
            Context.CreateEntity(Entity, entity =>
            {
                Player = entity.AddComponent<Player>();
                entity.AddComponent<HealOnBattleStart>();
                entity.AddComponent<PlayerUnit>();
                entity.AddComponent<VisualComponent>().AssetName = "Player";
                Health health = entity.AddComponent<Health>();
                health.DontDie = true;//DEBUG
                entity.AddComponent<Position>();
                health.SetMax(20);
                health.SetHealth(20);
            });
            CreateNewMap();

            CreatePrefabPile();

            //create an example deck.
            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            for (int i = 0; i < 2; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/ProtoField.json");
                Context.CreateEntity(Deck.Entity, "Cards/EnergyField.json");
                Context.CreateEntity(Deck.Entity, "Cards/Fidget.json");
                Context.CreateEntity(Deck.Entity, "Cards/ProtoPulse.json");
                Context.CreateEntity(Deck.Entity, "Cards/Pulse.json");
            }

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
                    Logging.LogError($"Failed to deserialize card {fileInfo.Name}. {e.Message}");
                }
            }
        }

        public void CreateDebugMap()
        {
            Player.Entity.TrySetParent(Context.Root);
            if (CurrentMap != null)
            {
                CurrentMap.Entity.Destroy();
            }

            Context.CreateEntity(Entity, entity =>
            {
                entity.AddComponent<DebugMapComponent>();
                CurrentMap = entity.GetComponent<CustomMap>();
            });
            foreach (CustomCell customCell in CurrentMap.GetAllCells())
            {
                if (customCell.IsWalkable)
                {
                    Player.Entity.GetComponent<Position>().Position1 = new Vector3(customCell.X, customCell.Y, 0);
                    break;
                }
            }

            foreach (CustomCell customCell in CurrentMap.GetAllCells().Reverse())
            {
                if (customCell.IsWalkable)
                {
                    CreateHatchInCell(customCell.Entity);
                    break;
                }
            }

            int enemies = 0;
            foreach (var prefab in GetEnemyInfos())
            {
                var cellsWithNoNeighbors = CurrentMap.GetAllCells()
                    .Where(c => !c.Entity.Children.Any() &&
                                CurrentMap.GetAdjacentCells(c.X, c.Y, true).All(neighbor => neighbor.Entity.Children.Count == 0));

                CreateEnemyInCell(cellsWithNoNeighbors.First().Entity, prefab);
            }
        }

        public void GoToNextLevel()
        {
            CurrentLevel++;
            CreateNewMap();
        }


        private void CreateNewMap()
        {
            Player.Entity.TrySetParent(Context.Root);
            if (CurrentMap != null)
            {
                CurrentMap.Entity.Destroy();
            }

            Context.CreateEntity(Entity, entity =>
            {
                entity.AddComponent<MapCreatorComponent>();
                CurrentMap = entity.GetComponent<CustomMap>();
            });
            foreach (CustomCell customCell in CurrentMap.GetAllCells())
            {
                if (customCell.IsWalkable)
                {
                    Player.Entity.GetComponent<Position>().Position1 = new Vector3(customCell.X, customCell.Y, 0);
                    break;
                }
            }

            foreach (CustomCell customCell in CurrentMap.GetAllCells().Reverse())
            {
                if (customCell.IsWalkable)
                {
                    CreateHatchInCell(customCell.Entity);
                    break;
                }
            }

            int enemies = 0;
            foreach (CustomCell customCell in CurrentMap.GetAllCells().Reverse())
            {
                if (!customCell.Entity.Children.Any())
                {
                    var neighbors = CurrentMap.GetAdjacentCells(customCell.X, customCell.Y).Where(n => !n.Entity.Children.Any()).ToList();
                    if (neighbors.Count == 2 && neighbors.All(n => n.X == customCell.X) || neighbors.All(n => n.Y == customCell.Y))
                    {
                        CreateRandomEnemyInCell(customCell.Entity);
                        enemies++;
                        if (enemies > 4)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void CreateHatchInCell(IEntity customCellEntity)
        {
            IEntity enemyEntity = Context.CreateEntity(customCellEntity, entity => { entity.AddComponent<Position>(); });
            enemyEntity.AddComponent<HatchEncounter>();
        }

        private void CreateRandomEnemyInCell(IEntity customCellEntity)
        {
            List<string> prefabs = GetEnemyInfos();
            int index = Random.SystemRandom.Next(prefabs.Count);
            string prefab = prefabs[index];
            CreateEnemyInCell(customCellEntity, prefab);
        }

        private List<string> GetEnemyInfos()
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Units"));
            List<string> files = info.GetFiles().Where(file => file.Extension == ".json").Select(file => $"Units/{file.Name}").ToList();
            return files;
        }

        private void CreateEnemyInCell(IEntity customCellEntity, string prefab)
        {
            IEntity enemyEntity = Context.CreateEntity(customCellEntity, entity => { entity.AddComponent<Position>(); });

            enemyEntity.AddComponent<BattleEncounter>().Prefab = prefab; //make random
            enemyEntity.AddComponent<VisualComponent>().AssetName = "DefaultEncounter";
        }

        private void AddRules()
        {
            Context.Root.AddComponent<DiscardHandOnTurnEnd>();
            Context.Root.AddComponent<DrawHandOnTurnBegin>();
        }

        public void EndTurn()
        {
            Events.OnTurnEnded(new TurnEndedEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }


        public void StartBattle(string prefab)
        {
            if (Battle != null)
            {
                Battle.Entity.Destroy();
            }

            Context.CreateEntity(Entity, entity => { Battle = entity.AddComponent<BattleContainer>(); });
            Battle.StartBattle(prefab);
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

    public class HatchEncounter : Encounter
    {
        protected override void PlayerEnteredCell()
        {
            Game.GoToNextLevel();
        }
    }

    public class BattleEncounter : Encounter
    {
        public string Prefab { get; set; }

        protected override void PlayerEnteredCell()
        {
            Game.StartBattle(Prefab);
            Entity.Destroy();
        }
    }

    public abstract class Encounter : SummerJam1Component
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
