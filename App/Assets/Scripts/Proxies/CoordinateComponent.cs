using System.ComponentModel;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using JetBrains.Annotations;
using UnityEngine;

namespace Proxies
{
    public class CoordinateComponent : ProxyComponent<ICoordinateProperty>
    {
        [SerializeField] private int SortOrder;

        [PropertyListener(nameof(ICoordinateProperty.Coordinate))]
        [UsedImplicitly]
        public void GameEntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (GameEntity.Context.GetCurrentBattle().Graph.TryGetNode(GameEntity.Coordinate, out var node))
            {
                var battle = GetComponentInParent<BattleProxy>();
                Vec2D worldCoord = GameEntity.Context.GetCurrentBattle().Graph.Grid
                    .AxialToPoint(node.Coordinate.ToAxial());
                transform.position = new Vector3(worldCoord.x, worldCoord.y, -SortOrder);
            }
        }
    }
}