namespace SummerJam1
{
    public class ArtifactHuntDungeonPile : DungeonPile
    {
        public override string Type => "Artifact Hunt";
        public override string Description => "Collect all pieces of the artifact to win.";

        private int NumFragments => Difficulty + 2;

        protected override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < NumFragments; i++)
            {
                Context.CreateEntity(Entity, child =>
                {
                    PrefabReference dungeon = child.AddComponent<PrefabReference>();
                    dungeon.Prefab = "Units/mcguffin.json";
                });
            }
        }
    }
}