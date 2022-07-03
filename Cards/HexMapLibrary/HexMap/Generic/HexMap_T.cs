using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class HexMapExtended : HexMap 
    {
        public TileDataProvider GetTile { get; private set; }
        public TilesDataProvider GetTiles { get; private set; }
        public Dictionary<Vector3Int, Tile> TilesByPosition { get; private set; }
        public Tile[] Tiles { get; private set; }

        public HexMapExtended()
        {
        }

        public HexMapExtended(Dictionary<Vector3Int, int> tileIndexByPosition, CoordinateWrapper coordinateWrapper = null) : base(tileIndexByPosition,
            coordinateWrapper)
        {
            Setup(tileIndexByPosition, coordinateWrapper);
        }

        public override void Setup(Dictionary<Vector3Int, int> tileIndexByPosition, CoordinateWrapper coordinateWrapper = null)
        {
            base.Setup(tileIndexByPosition, coordinateWrapper);
            CreateTileData();
            GetTile = new TileDataProvider(TilesByPosition, base.GetTilePosition);
            GetTiles = new TilesDataProvider(TilesByPosition, base.GetTilePositions);
        }

        public void CreateTileData()
        {
            Vector3 center = MapSizeData.center;
            Vector3 extents = MapSizeData.extents;
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            TilesByPosition = new Dictionary<Vector3Int, Tile>(vector3IntEqualityComparer);
            Tiles = new Tile[TileIndexByPosition.Count];
            float minX = center.x - extents.x;
            float maxX = center.x + extents.x;
            float minZ = center.z - extents.z;
            float maxZ = center.z + extents.z;

            foreach (var kvp in TileIndexByPosition)
            {
                Vector2 normalizedPosition = HexConverter.TileCoordToNormalizedPosition(kvp.Key, minX, maxX, minZ, maxZ);
                var tileEntity = Context.CreateEntity(Entity);
                var tile = tileEntity.AddComponent<Tile>();

                tile.Setup(kvp.Key, kvp.Value, normalizedPosition);
                TilesByPosition.Add(kvp.Key, tile);
                Tiles[tile.Index] = tile;
            }
        }
    }
}
