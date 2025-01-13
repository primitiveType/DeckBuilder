namespace SummerJam1
{
    public class ShopChoice : EncounterChoice
    {
        public override string Description { get; } = "Shop where you can cash in treasures and spend gold.";

        public override void Click()
        {
            Game.StartShop();
        }
    }
}