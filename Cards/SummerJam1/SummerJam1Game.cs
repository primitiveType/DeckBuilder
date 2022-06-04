using Api;

namespace SummerJam1
{
    public class SummerJam1Game : Component
    {
        private int NumSlots = 3;
        protected override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < NumSlots; i++)
            {
                IEntity slotEntity = Context.CreateEntity(Entity);
                slotEntity.AddComponent<UnitSlot>();
            }
        }
    }
}