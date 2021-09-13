using System;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class DamageIntent : Intent
    {
        private ActorNode m_TargetNode;
        public int DamageAmount { get; set; }

        public override string GetDescription =>
            Context.GetDamageAmount(this, DamageAmount, TargetNode.GetActor(),
                Owner.Entity).ToString();

        [JsonIgnore] public override ActorNode TargetNode => m_TargetNode;

        public override TargetingInfo Target { get; } = new HoneyCombTargeting();

        protected override void Initialize()
        {
            base.Initialize();
            var battle = Context.GetCurrentBattle();

            var playerCoord = Context.GetCurrentBattle().Player.Coordinate;
            var ownerCoord = Owner.Entity.Coordinate;
            var distance = ownerCoord.DistanceTo(playerCoord);
            if (Target.Range >= distance)
            {
                m_TargetNode = battle.Graph.GetNodeOfActor(battle.Player);
            }
            else
            {
                m_TargetNode = null;
                var line = ownerCoord.LineTo(playerCoord);
                if (Context.GetCurrentBattle().Graph.TryGetNode(line[Target.Range], out ActorNode node))
                {
                    m_TargetNode = node;
                }
            }
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

            foreach (var coord in Target.GetAffectedCoordinates(Owner.Entity.Coordinate, TargetNode.Coordinate))
            {
                if (battle.Graph.TryGetNode(coord, out ActorNode node))
                {
                    var actor = node.GetActor();
                    if (actor != null)
                    { //SHould probably change this to actually target nodes
                        Context.TryDealDamage(this, Owner.Entity, actor, DamageAmount);
                    }
                }
            }
        }
    }
}