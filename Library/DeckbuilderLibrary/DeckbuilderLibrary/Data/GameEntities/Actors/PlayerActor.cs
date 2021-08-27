using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public class PlayerActor : Actor
    {
        public int CurrentEnergy
        {
            get => Resources.GetResourceAmount<Energy>();
        }

        public int BaseEnergy
        {
            get => Resources.GetResourceAmount<BaseEnergy>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (!Resources.HasResource<BaseEnergy>())
            {
                Resources.SetResource<BaseEnergy>(3);
            }

            if (!Resources.HasResource<BaseCardDraw>())
            {
                Resources.SetResource<BaseCardDraw>(5);
            }
        }
    }
}