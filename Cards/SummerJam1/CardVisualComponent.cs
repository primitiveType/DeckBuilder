using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class VisualComponent : SummerJam1Component
    {
        public string AssetName { get; set; }
    }
    public class CardVisualComponent : SummerJam1Component
    {
        public SummerJam1CardAsset AssetName { get; set; }
    }
    public class RelicVisualComponent : SummerJam1Component
    {
        public SummerJam1RelicAsset AssetName { get; set; }
 
    }

    public class RelicComponent : SummerJam1Component, IPileItem
    {
        protected override void Initialize()
        {
            base.Initialize();
            Events.OnRelicCreated(new RelicCreatedEventArgs(Entity));
        }

        public bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public bool AcceptsChild(IEntity child)
        {
            return false;
        }
    }

}
