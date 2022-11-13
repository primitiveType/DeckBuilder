namespace SummerJam1
{
    public class
        ExterminationDungeonPile : DungeonPile
    {
        public override string Type => "Extermination";
        public override string Description => "Defeat all enemies to win.";

        protected override void Initialize()
        {
            base.Initialize();
            string dungeonPrefab = "dungeonList.json";
            //There's two steps here just to simplify the dungeon list json.
            var prefabsParent = Context.CreateEntity(null, dungeonPrefab);
            foreach (var child in prefabsParent.GetComponentsInChildren<PrefabReference>())
            {
                Context.CreateEntity(Entity, child.Prefab);
            }
        }
    }
}
