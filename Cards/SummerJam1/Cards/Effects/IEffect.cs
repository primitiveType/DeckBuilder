using Api;

namespace SummerJam1.Cards.Effects
{
    public interface IEffect
    {
        bool DoEffect(IEntity target);
    }
}