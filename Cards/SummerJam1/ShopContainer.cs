using Api;
using CardsAndPiles;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class ShopContainer : ShopSlotPile
    {
        private Game Game { get; set; }


        protected override void Initialize()
        {
            base.Initialize();
            Game = Context.Root.GetComponent<Game>();

            Logging.Log("Setting up shop...");
            for (int i = 0; i < 3; i++)
            {
                var card = Game.CreateRandomCard();
                Logging.Log($"Created card: {card.GetName()}");
                var success = card.TrySetParent(Entity);
                Logging.Log($"Added Card ? {success}");
                card.AddComponent<ClickToBuy>();
                var cost = card.AddComponent<Money>();
                cost.Amount = 40; //TODO: add rarities, base cost on that. Add OnSale component
            }
        }
    }
}