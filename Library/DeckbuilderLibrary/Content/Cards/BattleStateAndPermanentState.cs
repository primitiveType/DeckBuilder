using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.Property;
using DeckbuilderLibrary.Data.Property;
using DeckbuilderLibrary.Extensions;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class BattleStateAndPermanentState : EnergyCard
    {
        private string DataKey => nameof(BattleStateAndPermanentState) + "_" + Id;


        //any value tied to an id will be reset when a card is duplicated, as they get new ids
        //json properties will be copied when duplicating
        //battle properties will be copied when duplicating but reset when battle ends.
        //so to get the out-of-battle description of a card, we just create a true clone of it, end its battle, and check the description.

        [JsonProperty] public int TimesPlayed { get; set; }

        [JsonProperty] public BattleProperty<int> TimesPlayedThisCombat { get; } = new BattleProperty<int>(0);


        private int DamageIncreasePerPlay = 1;
        private int BaseDamage = 1;

        private int CurrentDamage => (TimesPlayed * DamageIncreasePerPlay) + BaseDamage;
        private int CurrentDamageThisCombat => (TimesPlayedThisCombat.Value * DamageIncreasePerPlay) + BaseDamage;


        public override string Name => nameof(BattleStateAndPermanentState);

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Deal {Context.GetDamageAmount(this, CurrentDamage, target as ActorNode, Owner)} to target enemy. Then deal {Context.GetDamageAmount(this, CurrentDamageThisCombat, target as ActorNode, Owner)}.";
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
            Context.TryDealDamage(this, Owner, target as ActorNode, CurrentDamageThisCombat);
            TimesPlayed += 1;
            TimesPlayedThisCombat.Value += 1;
        }


        public override int EnergyCost { get; } = 1;
    }
}