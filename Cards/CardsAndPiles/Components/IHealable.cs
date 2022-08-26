using Api;

namespace CardsAndPiles.Components
{
    public interface IHealable
    {
        int TryHeal(int damage, IEntity source);
    }
}
