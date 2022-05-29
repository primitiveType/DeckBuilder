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
}