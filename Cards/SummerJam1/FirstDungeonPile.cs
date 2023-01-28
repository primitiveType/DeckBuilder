using Api;

namespace SummerJam1
{
    public class
        FirstDungeonPile : DungeonPile
    {
        public override string Type => "Extermination";
        public override string Description => "Defeat all enemies to win.";

        protected override void Initialize()
        {
            base.Initialize();
            DungeonList = "dungeonList.json";

            IEntity prefabsParent = Context.CreateEntity(null, DungeonList);
            foreach (PrefabReference child in prefabsParent.GetComponentsInChildren<PrefabReference>())
            {
                Context.CreateEntity(Entity, child.Prefab);
            }
        }
    }
    public class
        SecondDungeonPile : DungeonPile
    {
        public override string Type => "Extermination";
        public override string Description => "Defeat all enemies to win.";

        protected override void Initialize()
        {
            base.Initialize();
            DungeonList = "icyDungeon.json";
            IEntity prefabsParent = Context.CreateEntity(null, DungeonList);
            foreach (PrefabReference child in prefabsParent.GetComponentsInChildren<PrefabReference>())
            {
                Context.CreateEntity(Entity, child.Prefab);
            }
        }
    }
}
