using Api;

namespace SummerJam1.Cards.Effects
{
    public interface IEffect
    {
        bool DoEffect(IEntity target);
        
        TargetingType Targeting { get; }
    }

    public enum TargetingType
    {
        None,
        Player,
        Unit,
        AllEnemies,
        RandomEnemy
    }
}