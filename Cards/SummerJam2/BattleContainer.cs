using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1;
using SummerJam1.Units;

namespace SummerJam2
{
    public class BattleContainer : SummerJam1Component
    {
        private int NumSlots = 3;
        public IEntity Discard { get; private set; }
        public IEntity Exhaust { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile BattleDeck { get; private set; }
        private IEntity SlotsParent { get; set; }

        public ObjectivesPile ObjectivesPile { get; private set; }


        protected override void Initialize()
        {
            base.Initialize();
            Context.CreateEntity(Entity, (entity) => ObjectivesPile = entity.AddComponent<ObjectivesPile>());

            SlotsParent = Context.CreateEntity(Entity);

            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(SlotsParent);
                slotEntity.AddComponent<EnemyUnitSlot>().Order = i;
            }

            //Context.CreateEntity(Entity, entity => BattleDeck = entity.AddComponent<DeckPile>());
            Discard = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerDiscard>());
            Exhaust = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerExhaust>());
        }


        public void StartBattle()
        {
            BattleDeck = Context.DuplicateEntity(Game.Deck.Entity).GetComponent<DeckPile>();
            BattleDeck.Entity.TrySetParent(Entity);
            Context.CreateEntity(Entity, entity =>
                Hand = entity.AddComponent<HandPile>());


            Events.OnBattleStarted(new BattleStartedEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }

        [OnEntityKilled]
        private void CheckForEndOfBattle(object sender, EntityKilledEventArgs args)
        {
            bool alliesAlive = args.Entity != Game.Player.Entity;
            bool enemiesAlive = SlotsParent.GetComponentsInChildren<EnemyUnitSlot>()
                .Any(slot =>
                    slot.Entity.Children.Any(child => child != args.Entity && child.GetComponent<Unit>() != null));

            if (alliesAlive && enemiesAlive)
            {
                return;
            }

            Events.OnBattleEnded(new BattleEndedEventArgs(alliesAlive));
            if (!alliesAlive)
            {
                Events.OnGameEnded(new GameEndedEventArgs(false));
            }
            else
            {
                Events.OnLeaveBattle(new LeaveBattleEventArgs());
            }
        }

        public IEntity GetFrontMostEnemy()
        {
            IOrderedEnumerable<EnemyUnitSlot> enemyUnitSlots =
                SlotsParent.GetComponentsInChildren<EnemyUnitSlot>().OrderBy(slot => slot.Order);
            return enemyUnitSlots.Select(slot => slot.Entity.GetComponentInChildren<Unit>()?.Entity).FirstOrDefault();
        }

        public IEntity GetFrontMostFriendly()
        {
            IOrderedEnumerable<FriendlyUnitSlot> friendlyUnitSlots =
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderBy(slot => slot.Order);
            return friendlyUnitSlots.Select(slot => slot.Entity.GetComponentInChildren<Unit>()?.Entity).FirstOrDefault();
        }

        public List<IEntity> GetFriendlies()
        {
            IEnumerable<IEntity> friendlyUnitSlots =
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderBy(slot => slot.Order)
                    .Select(slot => slot.Entity.GetComponentInChildren<Unit>()?.Entity).Where(unit => unit != null);
            return friendlyUnitSlots.ToList();
        }

        public List<IEntity> GetFullFriendlySlots()
        {
            IEnumerable<IEntity> friendlyUnitSlots =
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderBy(slot => slot.Order)
                    .Where(slot => slot.Entity.GetComponentInChildren<Unit>() != null).Select(slot => slot.Entity);
            return friendlyUnitSlots.ToList();
        }

        public List<IEntity> GetEnemies()
        {
            IEnumerable<IEntity> enemyUnitSlots =
                SlotsParent.GetComponentsInChildren<EnemyUnitSlot>().OrderBy(slot => slot.Order)
                    .Select(slot => slot.Entity.GetComponentInChildren<Unit>()?.Entity).Where(unit => unit != null);
            return enemyUnitSlots.ToList();
        }

        public List<IEntity> GetFullEnemySlots()
        {
            IEnumerable<IEntity> enemyUnitSlots =
                SlotsParent.GetComponentsInChildren<EnemyUnitSlot>().OrderBy(slot => slot.Order)
                    .Where(slot => slot.Entity.GetComponentInChildren<Unit>() != null).Select(slot => slot.Entity);
            return enemyUnitSlots.ToList();
        }

        public IEntity GetBackMostEmptySlot()
        {
            IOrderedEnumerable<FriendlyUnitSlot> enemyUnitSlots =
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderByDescending(slot => slot.Order);
            return enemyUnitSlots.FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() == null)?.Entity;
        }

        public IEntity GetFrontMostEmptySlot()
        {
            IOrderedEnumerable<FriendlyUnitSlot> enemyUnitSlots =
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderBy(slot => slot.Order);
            return enemyUnitSlots.FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() == null)?.Entity;
        }

        private List<IEntity> CreateRandomObjectives(int count = 2)
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Objectives"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();
            if (count > files.Count)
            {
                throw new ArgumentException(nameof(count));
            }

            List<int> indices = new List<int>(count);

            while (indices.Count < count)
            {
                int index = Game.Random.SystemRandom.Next(files.Count);
                if (!indices.Contains(index))
                {
                    indices.Add(index);
                }
            }

            List<IEntity> objectives = new List<IEntity>(count);
            foreach (int index in indices)
            {
                objectives.Add(Context.CreateEntity(null, Path.Combine("Objectives", files[index].Name)));
            }

            return objectives;
        }
    }
}
