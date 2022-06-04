using Api;

namespace CardsAndPiles
{
    public interface IPileConstraint : IComponent
    {
        bool CanReceive(IEntity itemView);
    }
}