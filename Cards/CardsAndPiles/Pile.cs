using Api;

namespace CardsAndPiles
{
    public abstract class Pile : Component, IPile
    {
        public abstract bool ReceiveItem(IPileItem itemView);

        public void RemoveItem(IPileItem itemView)
        {
        }


    }
}