using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace Content.Cards
{
    public abstract class EnergyCard : Card
    {
        public abstract int EnergyCost { get; }
        protected virtual PileType DefaultDestinationPile => PileType.DiscardPile;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += OnCardPlayed;
        }

        protected virtual void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, DefaultDestinationPile);
            }
        }

        public override bool IsPlayable()
        {
            return ((PlayerActor)Owner).CurrentEnergy >= EnergyCost;
        }

        protected override void DoPlayCard(IGameEntity target)
        {
            ((PlayerActor)Owner).Resources.SubtractResource<Energy>(EnergyCost);
        }
    }
}