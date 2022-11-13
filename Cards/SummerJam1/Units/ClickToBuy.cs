using Api;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Units
{
    public class ClickToBuy : PlayerCard, IClickable, IAmount
    {
        /// <summary>
        ///     The cost of the item.
        /// </summary>
        public int Amount { get; set; }
        
        public bool AllowMultiple { get; set; }

        public void Click()
        {
            Money wallet = Game.Player.Entity.GetComponent<Money>();
            if (wallet.Amount >= Amount)
            {
                wallet.Amount -= Amount;
                OnBuy();
                if (!AllowMultiple)
                {
                    Entity.Destroy();
                }
            }
        }

        protected virtual void OnBuy()
        {
            var prefab = Entity.GetComponent<PrefabReference>().Prefab;
            var bought = Context.CreateEntity(null, prefab);
            if (bought.HasComponent<Card>())
            {
                bought.TrySetParent(Game.Deck.Entity);
            }
            else if (bought.HasComponent<RelicComponent>())
            {
                bought.TrySetParent(Game.RelicPile.Entity);
            }
        }

        protected override bool PlayCard(IEntity target)
        {
            return false;
        }
    }
}