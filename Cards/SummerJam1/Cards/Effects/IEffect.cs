using Api;

namespace SummerJam1.Cards
{
    public interface IEffect
    {
        bool DoEffect(IEntity target);
    }
}