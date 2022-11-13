namespace SummerJam1.Units
{
    public class ShopDungeonPile : DungeonPile
    {
        public override string Type => "Merchant's Shop";
        public override string Description => "Purchase cards and relics!";

        protected override void Initialize()
        {
            base.Initialize();
            //only requirement is that this thing has children with SourcePrefabs indicating what gets created.
            //Better api for this can come later.

            int numItems = 4;
            for (int i = 0; i < numItems; i++)
            {
                Context.CreateEntity(Entity, newChild =>
                {
                    newChild.AddComponent<PrefabReference>().Prefab = Game.GetRandomCardSource();
                    ClickToBuy buyable = newChild.AddComponent<ClickToBuy>();
                    buyable.Amount = 50; //temp.
                });
            }
        }
    }
}