using System;
using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class HoneyCombTargeting : TargetingInfo
    {
        public override List<CubicHexCoord> GetCoordinates(CubicHexCoord targetCoord)
        {
            return targetCoord.Neighbors().Append(targetCoord).ToList();
        }
    }

    public class ClosestInRangeTargetingInfo : TargetingInfo
    {
        private readonly ICoordinateProperty m_Source;
        private readonly int m_Range;

        public ClosestInRangeTargetingInfo(ICoordinateProperty source, int range)
        {
            m_Source = source;
            m_Range = range;
        }

        public override List<CubicHexCoord> GetCoordinates(CubicHexCoord targetCoord)
        {
            int distance = m_Source.Coordinate.DistanceTo(targetCoord);
            if (m_Range >= distance)
            {
                return new List<CubicHexCoord> { targetCoord };
            }

            CubicHexCoord[] line = m_Source.Coordinate.LineTo(targetCoord);
            return new List<CubicHexCoord> { line[m_Range] };
        }
    }

    public class RangeTargeting : TargetingInfo
    {
        private readonly int m_Range;

        public RangeTargeting(int range)
        {
            m_Range = range;
        }

        public override List<CubicHexCoord> GetCoordinates(CubicHexCoord targetCoord)
        {
            return targetCoord.AreaAround(m_Range).ToList();
        }
    }

    public class SingleTargeting : TargetingInfo
    {
        public override List<CubicHexCoord> GetCoordinates(CubicHexCoord targetCoord)
        {
            return new List<CubicHexCoord> { targetCoord };
        }
    }

    public class RingTargeting : TargetingInfo
    {
        private readonly int m_Range;

        public RingTargeting(int range)
        {
            m_Range = range;
        }

        public override List<CubicHexCoord> GetCoordinates(CubicHexCoord targetCoord)
        {
            return targetCoord.RingAround(m_Range).ToList();
        }
    }

    public abstract class TargetingInfo
    {
        //what things do we care about?
        //get list of affected coordinates- requires targeted hex? requires owner location?

        //some infos take in a target hex and return a list of resulting hit hexes. 
        //others can be hovered and show affected hexes based on owner location.
        //all of them have a range though.

        //needs to provide some kind of generic info about what its doing to each hex. or can we just do simulated worlds?

        public virtual bool RequiresTarget { get; } = true;

        public virtual bool Follows { get; } = false; //probably shouldn't be part of the targeting info????

        public abstract List<CubicHexCoord>
            GetCoordinates(CubicHexCoord targetCoord);

        public List<ActorNode> GetNodes(CubicHexCoord targetCoord, IContext context)
        {
            List<ActorNode> nodes = new List<ActorNode>();
            foreach (var coord in GetCoordinates(targetCoord))
            {
                if (context.GetCurrentBattle().Graph.TryGetNode(coord, out ActorNode node))
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        //some attacks have a range and will hit X (1) number of targets within that range.
        //some attacks are a pre-declared aoe.
        //some attacks are of opportunity, but thats probably unrelated to targeting info.

        //unity asks an enemy at the start of turn what squares it is targeting/threatening.
        //are these the same thing? two totally different constructs?

        //idea
        //attacks declare a range, and whether they follow a target.
        //attacks that follow cause the ui to show the range of threat, and if the player is in that range it will also show
        //the threat.
        //attacks that do not follow will just show what squares are threatened.
        //is this gonna be a clusterfuck of a ui? I know ITB had to keep things really simple to avoid it being too noisy.
    }

    [Serializable]
    public abstract class Card : GameEntity
    {
        public abstract string Name { get; }

        public abstract string GetCardText(IGameEntity target = null);
        public abstract IReadOnlyList<IGameEntity> GetValidTargets();
        public abstract IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity target);

        public abstract bool RequiresTarget { get; }

        [JsonIgnore] public IActor Owner => Context.GetCurrentBattle().Player;


        public void PlayCard(IGameEntity target)
        {
            if (target == null && RequiresTarget)
            {
                throw new ArgumentException(
                    "This card requires a target, but an attempt was made to play it without one!");
            }

            if (RequiresTarget && !GetValidTargets().Contains(target))
            {
                throw new ArgumentException("Tried to play card on invalid target!");
            }

            DoPlayCard(target);
            ((IInternalBattleEventHandler)Context.Events).InvokeCardPlayed(this, new CardPlayedEventArgs(Id));
            if (!IsPlayable())
            {
                Console.WriteLine("Attempted to play card that was not playable!");
            }
        }


        protected abstract void DoPlayCard(IGameEntity target);

        public abstract bool IsPlayable();

        private void Log(string log)
        {
        }
    }
}