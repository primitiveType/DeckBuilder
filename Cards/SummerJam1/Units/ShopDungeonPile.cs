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
                string prefab = Game.GetRandomCardSource();

                Context.CreateEntity(Entity, prefab, newChild =>
                {
                    ClickToBuy buyable = newChild.AddComponent<ClickToBuy>();
                    Money cost = newChild.AddComponent<Money>();
                    cost.Amount = 50; //temp.
                });
            }

            Context.CreateEntity(Entity, "Units\\exit.json");
        }
    }
}
