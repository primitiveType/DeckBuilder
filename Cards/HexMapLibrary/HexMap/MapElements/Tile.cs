using UnityEngine;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    ///     non-generic base class for a tile containing positional data, either use generic version or extend from this class
    ///     for your own implementation
    /// </summary>
    public class Tile : MapElement
    {
        /// <summary>
        ///     returns the cartesian coordinate of the tile center
        /// </summary>
        public override Vector3 CartesianPosition => HexConverter.TileCoordToCartesianCoord(Position);
    }


    /// <summary>
    /// generic Tile which can have any object with a parameterless constructor as content
    /// </summary>    
    // public class Tile<T> : Tile where T : new()
    // {
    //     /// <summary>
    //     /// can be any object, defines the actual content of the Tile
    //     /// </summary>
    //     public T Data { get; set; }
    // }
}