using Api;
using RogueSharp;
using RogueSharp.MapCreation;

namespace RogueMaps
{
    public class MapCreatorComponent : Component
    {
        public Map Map { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Map = Map.Create(new RandomRoomsMapCreationStrategy<Map>(17, 10, 30, 5, 3));
            CustomMap customMap = Entity.AddComponent<CustomMap>();
            customMap.Initialize(Map.Width, Map.Height);
            foreach (Cell cell in Map.GetAllCells())
            {
                Context.CreateEntity(Entity, entity =>
                {
                    CustomCell newCell = entity.AddComponent<CustomCell>();
                    newCell.X = cell.X;
                    newCell.Y = cell.Y;
                    newCell.IsWalkable = cell.IsWalkable;
                    newCell.IsTransparent = cell.IsTransparent;
                    customMap[newCell.X, newCell.Y] = newCell;
                });
            }

            Entity.RemoveComponent(this);
        }
    }
}
