using System;
using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class DamageIntent : Intent
    {
        private TargetingInfo m_Target;
        public int DamageAmount { get; set; }

        public override string GetDescription =>
            Context.GetDamageAmount(this, DamageAmount, null,
                Owner.Entity).ToString();


        public TargetingInfo Affected { get; } = new HoneyCombTargeting();

        public override TargetingInfo Target => m_Target;

        protected override void Initialize()
        {
            base.Initialize();
            m_Target = new ClosestInRangeTargetingInfo(Owner.Entity, 4);
        }

        protected override void Trigger()
        {
            var battle = Context.GetCurrentBattle();
            if (Owner.Entity.Id == battle.Player.Id)
            {
                throw new NotImplementedException("Player intents not implemented.");
            }

            if (Owner.Entity.Id == -1)
            {
                throw new NotSupportedException("Intent with no owner was triggered!");
            }

            foreach (CubicHexCoord affected in GetAffectedCoords())
            {
                if (battle.Graph.TryGetNode(affected, out ActorNode node))
                {
                    Context.TryDealDamage(this, Owner.Entity, node, DamageAmount);
                }
            }
        }

        private CubicHexCoord GetTargetCoord()
        {
            return Context.GetCurrentBattle().Player.Coordinate;
        }

        public override List<CubicHexCoord> GetAffectedCoords()
        {
            List<CubicHexCoord> coords = new List<CubicHexCoord>();
            var targetCoords = Target.GetCoordinates(GetTargetCoord());
            foreach (var coord in targetCoords)
            {
                var affectedCoords = Affected.GetCoordinates(coord);
                foreach (var affected in affectedCoords)
                {
                    coords.Add(affected);
                }
            }

            return coords;
        }
    }
}