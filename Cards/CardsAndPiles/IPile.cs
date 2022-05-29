using Api;

namespace CardsAndPiles
{
    public interface IPile : IComponent
    {
    
        bool ReceiveItem(IPileItem itemView);
        void RemoveItem(IPileItem itemView);
    }
}