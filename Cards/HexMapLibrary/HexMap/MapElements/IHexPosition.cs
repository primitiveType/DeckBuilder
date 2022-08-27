using UnityEngine;

namespace Wunderwunsch.HexMapLibrary
{
    public interface IHexPosition
    {
        Vector3Int Position { get; }
        Vector3 CartesianPosition { get; }
    }
}
