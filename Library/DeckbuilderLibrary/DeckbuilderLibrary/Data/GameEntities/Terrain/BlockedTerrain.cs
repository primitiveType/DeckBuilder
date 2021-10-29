using System.ComponentModel;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data.GameEntities.Terrain
{
    public class CoordinateEntity : GameEntity, IInternalCoordinateProperty
    {
        private CubicHexCoord m_Coordinate;
        public CubicHexCoord Coordinate => m_Coordinate;

        CubicHexCoord IInternalCoordinateProperty.Coordinate
        {
            get => m_Coordinate;
            set => SetField(ref m_Coordinate, value);
        }
    }

    public class BlockedTerrain : CoordinateEntity, IBlocksMovement
    {
    }

    public class FireTerrain : CoordinateEntity, IInternalCoordinateProperty
    {
        private int Damage => 5;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (Context.GetCurrentBattle().Graph.TryGetNode(Coordinate, out ActorNode node))
            {
                Context.TryDealDamage(this, null, node, Damage);
            }
        }
    }

    public class Collectible : CoordinateEntity, IInternalCoordinateProperty
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.GetCurrentBattle().Player.AddListener(PlayerOnPropertyChanged);
        }


        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerActor.Coordinate))
            {
                var player = Context.GetCurrentBattle().Player;
                if (player.Coordinate.Equals(Coordinate))
                {
                    if (Context.GetCurrentBattle().Graph.TryGetNode(Coordinate, out var node))
                    {
                        node.TryRemove(this);
                        player.Resources.AddResource<Energy>(1);
                        Destroy();
                    }
                }
            }
        }
    }
}