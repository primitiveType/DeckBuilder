using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources.Status;

namespace Content.Cards
{
    public class Bash : EnergyCard
    {
        public override string Name => nameof(Bash);

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += OnCardPlayed;
        }

        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }

        private int DamageAmount = 8;
        private int VulnerableAmount = 2;

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)}. Apply {this.VulnerableAmount} vulnerable.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;
        public override int EnergyCost => 2;

        protected override void DoPlayCard(IGameEntity target)
        {
            // Deal x damage.
            Context.TryDealDamage(this, Owner, target as IActor, DamageAmount);
            // Apply y vulnerable.
            ((IActor)target).Resources.AddResource<VulnerableStatusEffect>(VulnerableAmount);
        }
    }
}