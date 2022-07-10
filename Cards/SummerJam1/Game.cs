using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public MapComponent CurrentMap { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            AddRules();
            Random = Entity.AddComponent<Random>();
            Context.CreateEntity(Entity, entity => PrizePile = entity.AddComponent<SummerJam1PrizePile>());
            Context.CreateEntity(Entity, entity => RelicPrizePile = entity.AddComponent<SummerJam1RelicPrizePile>());
            Context.CreateEntity(Entity, entity => RelicPile = entity.AddComponent<RelicPile>());
            Context.CreateEntity(Entity, entity => { PlayerSlot = entity.AddComponent<UnitSlot>(); });
            Context.CreateEntity(Entity, entity =>
            {
                Player = entity.AddComponent<Player>();
                entity.AddComponent<PlayerUnit>();
                entity.AddComponent<VisualComponent>().AssetName = "Player";
                Health health = entity.AddComponent<Health>();
                entity.AddComponent<Position>();
                health.SetMax(50);
                health.SetHealth(50);
            });
            CreateNewMap();

            //create an example deck.
            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            for (int i = 0; i < 50; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/Starting/Honey.json");
            }

            Events.OnGameStarted(new GameStartedEventArgs());
        }

        private void CreateNewMap()
        {
            if (CurrentMap != null)
            {
                CurrentMap.Entity.Destroy();
            }

            Context.CreateEntity(Entity, entity => { CurrentMap = entity.AddComponent<MapComponent>(); });
            foreach (CustomCell customCell in CurrentMap.Map.GetAllCells())
            {
                if (customCell.IsWalkable)
                {
                    Player.Entity.GetComponent<Position>().Position1 = new Vector3(customCell.X, 0, customCell.Y);
                }
            }
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


        public void StartBattle()
        {
            if (Battle != null)
            {
                Battle.Entity.Destroy();
            }

            Context.CreateEntity(Entity, (entity) => { Battle = entity.AddComponent<BattleContainer>(); });
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
}
