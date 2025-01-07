namespace SummerJam1
{
    public class BuyableCard : SummerJam1Component, IBuyable
    {
        public int Cost { get; set; }

        public void Buy()
        {
            Money wallet = Game.Player.Entity.GetOrAddComponent<Money>();
            if (wallet.Amount >= Cost)
            {
                wallet.Amount -= Cost;
                Entity.TrySetParent(Game.Deck.Entity);
                Entity.RemoveComponent<BuyableCard>();
            }
        }
    }
}