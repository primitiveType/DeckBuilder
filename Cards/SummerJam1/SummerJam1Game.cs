using Api;
using CardsAndPiles;
using Component = Api.Component;

namespace SummerJam1
{
    public class SummerJam1Game : Component
    {
        private int NumSlots = 3;
        public IEntity Discard { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(Entity);
                slotEntity.AddComponent<FriendlyUnitSlot>();
            }

            IEntity deck = Context.CreateEntity(Entity, entity => entity.AddComponent<DeckPile>());
            Discard = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerDiscard>());
            for (int i = 0; i < 20; i++)
            {
                Context.CreateEntity(deck, entity => { entity.AddComponent<StarterUnitCard>(); });
            }

            Context.CreateEntity(Entity, entity => entity.AddComponent<HandPile>());
        }
    }

   

}