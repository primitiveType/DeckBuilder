using Api;

namespace CardsAndPiles
{
    public interface IPileConstraint : IComponent
    {
        bool CanReceive(Entity itemView);
    }
}