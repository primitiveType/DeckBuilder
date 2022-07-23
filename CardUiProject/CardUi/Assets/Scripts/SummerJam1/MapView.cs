using System.ComponentModel;
using App;
using CardsAndPiles.Components;
using RogueMaps;
using SummerJam1;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapView : View<CustomMap>
{
    [SerializeField] private GameObject WalkablePrefab;
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject HatchEncounterPrefab;
    [SerializeField] private GameObject BattleEncounterPrefab;

    private void Awake()
    {
        SetModel(GameContext.Instance.Game.CurrentMap.Entity);
        CreateMap();
        GameContext.Instance.Game.PropertyChanged += GameOnPropertyChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameContext.Instance.Game.PropertyChanged -= GameOnPropertyChanged;
    }

    private void GameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GameContext.Instance.Game.CurrentMap))
        {
            SceneManager.LoadScene("Scenes/SummerJam1/MapScene");
        }
    }

    private void CreateMap()
    {
        foreach (CustomCell cell in Model.GetAllCells()) //TODO: should cells have their own view?
        {
            GameObject prefab = cell.IsWalkable ? WalkablePrefab : WallPrefab;
            var tile = Instantiate(prefab, transform);
            tile.transform.localPosition = new Vector3(cell.X, cell.Y);
            tile.name += $"{cell.X}, {cell.Y}"; 
            foreach (Encounter encounter in cell.Entity.GetComponentsInChildren<Encounter>())
            {
                GameObject go = Instantiate(BattleEncounterPrefab, transform);
                go.GetComponent<ISetModel>().SetModel(encounter.Entity);
                go.name = $"Encounter {encounter.Entity.GetComponent<Position>().Position1.ToString()}";
            }
        }
    }
}
