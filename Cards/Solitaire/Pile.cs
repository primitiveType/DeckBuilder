using Api;
using CardsAndPiles;

namespace Solitaire
{
    public abstract class Pile : Component, IPile
    {
        public abstract bool ReceiveItem(IPileItem itemView);

        public void RemoveItem(IPileItem itemView)
        {
        }

    }
}