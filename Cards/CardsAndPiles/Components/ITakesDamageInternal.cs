using Api;

namespace CardsAndPiles.Components
{
    internal interface ITakesDamageInternal : ITakesDamage
    {
        void DealDamage(int damage, IEntity source);
    }
}
