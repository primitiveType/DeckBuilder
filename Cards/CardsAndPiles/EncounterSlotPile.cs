using System.Linq;
using Api;

namespace CardsAndPiles
{
    public class ShopSlotPile : DefaultPile
    {
        public override bool AcceptsChild(IEntity child)
        {
             return base.AcceptsChild(child) && !Entity.Children.Any();
        }
    }
    public class EncounterSlotPile : DefaultPile
    {
    }
    public class UpcomingEncounterSlotPile : DefaultPile
    {
    }
}
