using System.Collections.Generic;
using System.ComponentModel;
using Api;
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

    void Awake()
    {
        SetModel(SummerJam1Context.Instance.Game.CurrentMap.Entity);
        CreateMap();
        SummerJam1Context.Instance.Game.PropertyChanged += GameOnPropertyChanged;
    }

    private void GameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SummerJam1Context.Instance.Game.CurrentMap))
        {
            SceneManager.LoadScene("Scenes/SummerJam1/MenuScene");
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SummerJam1Context.Instance.Game.PropertyChanged -= GameOnPropertyChanged;
    }

    private void CreateMap()
    {
        foreach (var cell in Model.GetAllCells()) //TODO: should cells have their own view?
        {
            var prefab = cell.IsWalkable ? WalkablePrefab : WallPrefab;
            Instantiate(prefab, transform);
            prefab.transform.localPosition = new Vector3(cell.X, cell.Y);

            foreach (Encounter encounter in cell.Entity.GetComponentsInChildren<Encounter>())
            {
                var go = Instantiate(BattleEncounterPrefab, transform);
                go.GetComponent<ISetModel>().SetModel(encounter.Entity);
                go.name = $"Encounter {encounter.Entity.GetComponent<Position>().Position1.ToString()}";
            }
 
        }
    }
}
