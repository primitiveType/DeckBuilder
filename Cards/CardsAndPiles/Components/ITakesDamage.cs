using Api;

namespace CardsAndPiles.Components
{
    public interface ITakesDamage : IComponent
    {
        int TryDealDamage(int damage, IEntity source);
        int GetEffectiveDamage(int damage, IEntity source);
    }
}
