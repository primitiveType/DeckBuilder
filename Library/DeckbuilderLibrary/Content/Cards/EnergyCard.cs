using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace Content.Cards
{
    public abstract class EnergyCard : Card
    {
        public abstract int EnergyCost { get; }

        public override bool IsPlayable()
        {
            return ((PlayerActor)Owner).CurrentEnergy >= EnergyCost;
        }

        protected override void DoPlayCard(IActor target)
        {
            ((PlayerActor)Owner).Resources.SubtractResource<Energy>(EnergyCost);
        }
    }
}