using UnityEngine;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    ///     non-generic base class for an edge containing positional data, either use generic version or extend from this class
    ///     for your own implementation
    /// </summary>
    public class Edge : MapElement
    {
        /// <summary>
        ///     Initializes a new Edge instance
        /// </summary>
        /// >
        public Edge(Vector3Int position, int index, Vector2 normalizedPosition) : base(position, index, normalizedPosition)
        {
            EdgeAlignment = HexUtility.GetEdgeAlignment(position);
        }

        /// <summary>
        ///     returns the cartesian coordinate of the edge midpoint
        /// </summary>
        public override Vector3 CartesianPosition => HexConverter.EdgeCoordToCartesianCoord(Position);

        /// <summary>
        ///     the edge is parallel to this axis
        /// </summary>
        public EdgeAlignment EdgeAlignment { get; }

        /// <summary>
        ///     the rotationAngle of the edge depending on EdgeOrientation,
        /// </summary>
        public float EdgeAlignmentAngle => HexUtility.anglebyEdgeAlignment[EdgeAlignment];
    }
}