using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Extensions;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DealMoreDamageEachPlay : EnergyCard
    {
        [JsonProperty] public int TimesPlayed { get; set; }

        private int DamageIncreasePerPlay = 1;
        private int BaseDamage = 1;

        private int CurrentDamage => (TimesPlayed * DamageIncreasePerPlay) + BaseDamage;


        public override string Name => nameof(DealMoreDamageEachPlay);

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Deal {Context.GetDamageAmount(this, CurrentDamage, target as ActorNode, Owner)} to target enemy. Increase this card's damage by 1 for the rest of combat.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetEnemies().Select(e => e.Node).ToList();
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            Context.TryDealDamage(this, Owner, target as ActorNode, CurrentDamage);
            TimesPlayed += 1;
        }


        public override int EnergyCost { get; } = 1;
    }
}