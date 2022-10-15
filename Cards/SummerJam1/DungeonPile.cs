using Api;

namespace SummerJam1
{
    public abstract class DungeonPile : SummerJam1Component, IVisual
    {
        public abstract string Type { get; }
        public abstract string Description { get; }
        public virtual int RequiredCards => 15;

        protected virtual int NumBoosters => 3;

        public int Difficulty { get; set; }

        protected virtual int GetMinCards(int difficulty)
        {
            return 10 + difficulty;
        }

        protected virtual int GetMaxCards(int difficulty)
        {
            return 15 + difficulty;
        }

        protected override void Initialize()
        {
            base.Initialize();

            string dungeonPrefab = "dungeonList.json";
            Context.CreateEntity(Entity, dungeonPrefab);
            //     {
            // if (File.Exists(dungeonPrefab))
            // {
            //     var monsters = Serializer.Deserialize<IEntity>(File.ReadAllText(dungeonPrefab));
            //     
            // }
            // else
            // {
            //     string[] monsters = new[] { "Units/Standard/1/birthdayBoy.json" };
            //     var pl = new PrefabList();
            //     pl.Prefabs = monsters;
            //     string serialize = Serializer.Serialize(pl);
            //     File.WriteAllText(dungeonPrefab, serialize);
            // }

            // for (int i = 0; i < NumBoosters; i++)
            // {
            //     Context.CreateEntity(Entity, child =>
            //     {
            //         EncounterBoosterPack pack = child.AddComponent<EncounterBoosterPack>();
            //         foreach (string prefab in Game.GetBattlePrefabs(2, 5))
            //         {
            //             Context.CreateEntity(pack.Entity, card =>
            //             { //add each card to the booster pack.
            //                 PrefabReference cardToAdd = card.AddComponent<PrefabReference>();
            //                 cardToAdd.Prefab = prefab;
            //             });
            //         }
            //     });
            // }
        }
    }
}
