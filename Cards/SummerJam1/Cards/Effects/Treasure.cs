using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class Treasure : SummerJam1Component, IDescription
    {
        protected override void Initialize()
        {
            base.Initialize();
            var parent = Entity.Parent;
            while (parent != null)
            {
                Logging.Log(parent.GetName());
                parent = parent.Parent;
            }
        }

        [OnShopStarted]
        private void OnShopStarted(object sender, ShopStartedEventArgs args)
        {
            Money wallet = Game.Player.Entity.GetComponent<Money>();
            Money reward = Entity.GetComponent<Money>();
            wallet.Amount += reward.Amount;
            var parent = Entity.Parent;
            while (parent != null)
            {
                Logging.Log(parent.GetName());
                parent = parent.Parent;
            }
            Entity.Destroy();
        }

        public string Description { get; } = "Treasure. Redeemed at Shops.";

    }
}