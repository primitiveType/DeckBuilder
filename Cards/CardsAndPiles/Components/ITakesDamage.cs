using Api;

namespace CardsAndPiles.Components
{
    public interface ITakesDamage 
    {
        int TryDealDamage(int damage, IEntity source);
    }

    public interface IHealable
    {
        int TryHeal(int damage, IEntity source);

    }

    internal interface ITakesDamageInternal : ITakesDamage
    {
        void DealDamage(int damage, IEntity source);
    }
}