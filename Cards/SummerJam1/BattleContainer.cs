using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1.Units;

namespace SummerJam1
{
    public class SummerJam1PrizePile : PrizePile
    {
 

        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                //Player.Entity.TrySetParent(TempPlayerSlot.Entity);
                SetupPrizePile();
            }
        }

        public void SetupPrizePile()
        {
            Clear();
            for (int i = 0; i < 3; i++)
            {
                Entity.GetComponentInParent<SummerJam1Game>().CreateRandomCard().TrySetParent(Entity);
            }
        }

        public void ChoosePrize(IEntity child)
        {
            child.TrySetParent(Entity.GetComponentInParent<SummerJam1Game>().Deck.Entity);

            Clear();
        }

        private void Clear()
        {
            foreach (IEntity unwantedChild in Entity.Children.ToList())
            {
                unwantedChild.Destroy();
            }
        }
    }
    public class BattleContainer : SummerJam1Component
    {
        private int NumSlots = 3;
        public IEntity Discard { get; private set; }
        public IEntity Exhaust { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile BattleDeck { get; private set; }
        private IEntity SlotsParent { get; set; }

        private SummerJam1Game Game { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            SlotsParent = Context.CreateEntity(Entity);
            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(SlotsParent);
                slotEntity.AddComponent<FriendlyUnitSlot>().Order = i;
            }

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
            Game = Entity.GetComponentInParent<SummerJam1Game>();
            BattleDeck = Context.DuplicateEntity(Game.Deck.Entity).GetComponent<DeckPile>();
            Context.CreateEntity(Entity, entity =>
                Hand = entity.AddComponent<HandPile>());

            int i = 0;
            foreach (EnemyUnitSlot slot in Entity.GetComponentsInChildren<EnemyUnitSlot>())
            {
                if (i == 0)
                {
                    Context.CreateEntity(slot.Entity, "Units/noodles.json");
                }

                if (i == 1)
                {
                    Context.CreateEntity(slot.Entity, "Units/headcheese.json");
                }
                
                if (i == 2)
                {
                    Context.CreateEntity(slot.Entity, "Units/sandwich.json");
                }

                i++;
            }

            Events.OnBattleStarted(new BattleStartedEventArgs(this));
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
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderBy(slot => slot.Order).Where(slot=>slot.Entity.GetComponentInChildren<Unit>() != null).Select(slot=>slot.Entity);
            return friendlyUnitSlots.ToList();
        }
        
        public IEntity GetBackMostEmptySlot()
        {
            IOrderedEnumerable<FriendlyUnitSlot> enemyUnitSlots =
                SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>().OrderByDescending(slot => slot.Order);
            return enemyUnitSlots.FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() == null)?.Entity;
        }
    }
}