using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public abstract class DungeonPile : SummerJam1Component, IVisual
    {
        public abstract string Type { get; }
        public abstract string Description { get; }
        
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
            foreach (string prefab in Game.GetBattlePrefabs(GetMinCards(Difficulty), GetMaxCards(Difficulty))) //temp code, all dungeons will be the same. 
            {
                PrefabReference dungeon = null;
                Context.CreateEntity(Entity, child => dungeon = child.AddComponent<PrefabReference>());
                dungeon.Prefab = prefab;
            }
        }
    }
}
