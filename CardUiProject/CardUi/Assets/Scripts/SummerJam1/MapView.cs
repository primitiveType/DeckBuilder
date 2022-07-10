using App;
using RogueMaps;
using SummerJam1;
using UnityEngine;

public class MapView : View<MapComponent>
{
    [SerializeField] private GameObject WalkablePrefab;
    [SerializeField] private GameObject WallPrefab;
    void Awake()
    {
        SetModel(SummerJam1Context.Instance.Game.CurrentMap.Entity);
        CreateMap();
    }

    private void CreateMap()
    {
        foreach (var cell in Model.Map.GetAllCells())
        {
            var prefab = cell.IsWalkable ? WalkablePrefab : WallPrefab;
            Instantiate(prefab, transform);
            prefab.transform.localPosition = new Vector3(cell.X, 0, cell.Y);
        }
    }
    
}
