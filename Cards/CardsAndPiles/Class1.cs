using System;
using Api;

namespace CardsAndPiles
{
    public interface IPileItem : IComponent
    {
        //move to Ipileitem
        // bool CanEnterPile(IPile pile);
        bool TrySendToPile(IPile pile);
    }
    
    public interface IPile : IComponent
    {
    
        bool ReceiveItem(IPileItem itemView);
        void RemoveItem(IPileItem itemView);
    }
    public interface IPileConstraint : IComponent
    {
        bool CanReceive(Entity itemView);
    }
}