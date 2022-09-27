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


            for (int i = 0; i < NumBoosters; i++)
            {
                Context.CreateEntity(Entity, child =>
                {
                    EncounterBoosterPack pack = child.AddComponent<EncounterBoosterPack>();
                    foreach (string prefab in Game.GetBattlePrefabs(2, 5))
                    {
                        Context.CreateEntity(pack.Entity, card =>
                        { //add each card to the booster pack.
                            PrefabReference cardToAdd = card.AddComponent<PrefabReference>();
                            cardToAdd.Prefab = prefab;
                        });
                    }
                });
            }
        }
    }
}