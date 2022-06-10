using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Rules;
using SummerJam1.Units;

namespace SummerJam1
{
    public class SummerJam1Game : SummerJam1Component
    {
        private int NumSlots = 3;
        public IEntity Discard { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile Deck { get; private set; }

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

            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            Discard = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerDiscard>());
            for (int i = 0; i < 20; i++)
            {
                Context.CreateEntity(Deck.Entity, entity =>
                {
                    entity.AddComponent<StarterUnitCard>();
                    entity.AddComponent<NameComponent>().Value = "Pepper";
                });
                Context.CreateEntity(Deck.Entity, "Dice.json");
                Context.CreateEntity(Deck.Entity, "DrawNewHand.json");
            }

            Context.CreateEntity(Entity, entity =>
                Hand = entity.AddComponent<HandPile>());

            AddRules();
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
            foreach (EnemyUnitSlot slot in Entity.GetComponentsInChildren<EnemyUnitSlot>())
            {
                Context.CreateEntity(slot.Entity, "noodles.json");
            }

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