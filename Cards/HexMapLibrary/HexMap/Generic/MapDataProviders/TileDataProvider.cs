using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class TileDataProvider
    {
        private Dictionary<Vector3Int, Tile> tilesByPosition;
        private TilePositionProvider tilePositionProvider;

        public TileDataProvider(Dictionary<Vector3Int, Tile> tilesByPosition, TilePositionProvider tilePositionProvider)
        {
            this.tilesByPosition = tilesByPosition;
            this.tilePositionProvider = tilePositionProvider;
        }

        /// <summary>
        /// TODO add more description, returns null if it the input coordinate is not on the map;
        /// </summary>
        /// <param name="cartesianCoordinate"></param>
        /// <returns></returns>
        public Tile FromCartesianCoordinate(Vector3 cartesianCoordinate)
        {
            Vector3Int coord = tilePositionProvider.FromCartesianCoordinate(cartesianCoordinate);
            if (!tilesByPosition.ContainsKey(coord)) return null;
            return tilesByPosition[coord];
        }

        /// <summary>
        /// rotates the input tile coordinate 60° Clockwise around the specified center point and returns the tile on that position. returns null if outside of map bounds
        /// </summary>
        public Tile FromTileRotated60DegreeClockwise(Vector3Int center, Vector3Int pointToRotate)
        {
            Vector3Int coord = tilePositionProvider.FromTileRotated60DegreeClockwise(center, pointToRotate);
            if (!tilesByPosition.ContainsKey(coord)) return null;
            return tilesByPosition[coord];
        }

        /// <summary>
        /// rotates the input tile coordinate 60° Clockwise around the specified center point and returns the tile on that position. returns null if outside of map bounds
        /// </summary>
        public Tile FromTileRotated60DegreeClockwise(Tile center, Tile pointToRotate)
        {
            Vector3Int coord = tilePositionProvider.FromTileRotated60DegreeClockwise(center.Position, pointToRotate.Position);
            if (!tilesByPosition.ContainsKey(coord)) return null;
            return tilesByPosition[coord];
        }

        /// <summary>
        /// rotates the input tile coordinate 60° Counter-clockwise around the specified center point and returns the tile on that position. returns null if outside of map bounds
        /// </summary>
        public Tile FromTileRotated60DegreeCounterClockwise(Vector3Int center, Vector3Int pointToRotate)
        {
            Vector3Int coord = tilePositionProvider.FromTileRotated60DegreeCounterClockwise(center, pointToRotate);
            if (!tilesByPosition.ContainsKey(coord)) return null;
            return tilesByPosition[coord];
        }

        /// <summary>
        /// rotates the input tile coordinate 60° Counter-clockwise around the specified center point and returns the tile on that position. returns null if outside of map bounds
        /// </summary>
        public Tile FromTileRotated60DegreeCounterClockwise(Tile center, Tile pointToRotate)
        {
            Vector3Int coord = tilePositionProvider.FromTileRotated60DegreeCounterClockwise(center.Position, pointToRotate.Position);
            if (!tilesByPosition.ContainsKey(coord)) return null;
            return tilesByPosition[coord];
        }
    }
}
