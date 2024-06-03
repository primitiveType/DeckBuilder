using Api;

namespace CardsAndPiles.Components
{
    public class CardsAndPilesComponent : Component
    {
        protected new CardEvents Events => (CardEvents)base.Events;
    }
}