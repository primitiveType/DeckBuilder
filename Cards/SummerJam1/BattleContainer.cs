using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1.Units;

namespace SummerJam1
{
    public class BattleContainer : SummerJam1Component
    {
        private int NumSlots = 3;
        public IEntity Discard { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile BattleDeck { get; private set; }
        private IEntity SlotsParent { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            

            SlotsParent = Context.CreateEntity(Entity);
            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(SlotsParent);
                slotEntity.AddComponent<FriendlyUnitSlot>();
            }

            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(SlotsParent);
                slotEntity.AddComponent<EnemyUnitSlot>();
            }

            Context.CreateEntity(Entity, entity => BattleDeck = entity.AddComponent<DeckPile>());
            Discard = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerDiscard>());

        }

        public void StartBattle()
        {
            SummerJam1Game game = Entity.GetComponentInParent<SummerJam1Game>();
            BattleDeck = Context.DuplicateEntity(game.Deck.Entity).GetComponent<DeckPile>();
            Context.CreateEntity(Entity, entity =>
                Hand = entity.AddComponent<HandPile>());

            foreach (EnemyUnitSlot slot in Entity.GetComponentsInChildren<EnemyUnitSlot>())
            {
                Context.CreateEntity(slot.Entity, "noodles.json");
            }

            Events.OnBattleStarted(new BattleStartedEventArgs(this));
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }

        [OnEntityKilled]
        private void CheckForEndOfBattle(object sender, EntityKilledEventArgs args)
        {
            bool alliesAlive = SlotsParent.GetComponentsInChildren<FriendlyUnitSlot>()
                .Any(slot =>
                    slot.Entity.Children.Any(child => child != args.Entity && child.GetComponent<Unit>() != null));
            bool enemiesAlive = SlotsParent.GetComponentsInChildren<EnemyUnitSlot>()
                .Any(slot =>
                    slot.Entity.Children.Any(child => child != args.Entity && child.GetComponent<Unit>() != null));

            if (alliesAlive && enemiesAlive)
            {
                return;
            }

            Events.OnBattleEnded(new BattleEndedEventArgs(alliesAlive));
        }
    }
}