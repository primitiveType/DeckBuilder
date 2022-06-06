using Api;
using CardsAndPiles;
using SummerJam1.Rules;
using SummerJam1.Units;

namespace SummerJam1
{
    public class SummerJam1Game : SummerJam1Component
    {
        private int NumSlots = 3;
        public IEntity Discard { get; private set; }
        public IEntity Hand { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(Entity);
                slotEntity.AddComponent<FriendlyUnitSlot>();
            }

            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(Entity);
                slotEntity.AddComponent<EnemyUnitSlot>();
            }

            IEntity deck = Context.CreateEntity(Entity, entity => entity.AddComponent<DeckPile>());
            Discard = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerDiscard>());
            for (int i = 0; i < 20; i++)
            {
                Context.CreateEntity(deck, entity => { entity.AddComponent<StarterUnitCard>(); });
            }

            Hand = Context.CreateEntity(Entity, entity => entity.AddComponent<HandPile>());

            Context.Root.AddComponent<DiscardHandOnTurnEnd>();
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
                Context.CreateEntity(slot.Entity, entity => { entity.AddComponent<StarterUnit>(); });
            }
            
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }
    }
}