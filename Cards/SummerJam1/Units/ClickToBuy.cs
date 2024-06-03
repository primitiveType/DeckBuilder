using Api;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Units
{
    public class ClickToBuy : PlayerCard, IClickable
    {
        public bool AllowMultiple { get; set; }

        public void Click()
        {
            Money cost = Entity.GetComponent<Money>();
            Money wallet = Game.Player.Entity.GetComponent<Money>();
            if (wallet.Amount >= cost.Amount)
            {
                wallet.Amount -= cost.Amount;
                OnBuy();
                if (!AllowMultiple)
                {
                    Entity.Destroy();
                }
            }
        }

        protected virtual void OnBuy()
        {
            string prefab = Entity.GetComponent<SourcePrefab>().Prefab;
            IEntity bought = Context.CreateEntity(null, prefab);
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
