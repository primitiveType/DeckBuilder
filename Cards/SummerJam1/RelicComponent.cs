using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class RelicComponent : SummerJam1Component, IPileItem, IVisual
    {
        public bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public bool AcceptsChild(IEntity child)
        {
            return false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Events.OnRelicCreated(new RelicCreatedEventArgs(Entity));
        }
    }
}