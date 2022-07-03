using System;
using Api;
using App.Utility;
using CardsAndPiles;
using SummerJam1;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using UnityEngine;

public class HexesManager : MonoBehaviourSingleton<HexesManager>
{
    private HexMapExtended Game { get; set; }
    private Context Context { get; set; }
    [SerializeField] private int _size = 10;
    [SerializeField] private HexView m_HexView;

    protected override void SingletonAwakened()
    {
        Context = new Context(new CardEvents());
        IEntity gameEntity = Context.Root;
        Context.SetPrefabsDirectory("StreamingAssets");
        Game = gameEntity.AddComponent<HexMapExtended>();
        Game.Setup(HexMapBuilder.CreateHexagonalShapedMap(_size));
        RecursivelySetup(Game.Entity);
    }

    private void RecursivelySetup(IEntity root)
    {
        CreateGameObjectForModel(root);

        foreach (var child in root.Children)
        {
            RecursivelySetup(child);
        }
    }

    public GameObject CreateGameObjectForModel(IEntity entity)
    {
        if (entity.GetComponent<Tile>() != null)
        {
            var hex = Instantiate(m_HexView);
            hex.SetModel(entity);
            return hex.gameObject;
        }

        return null;
    }
}
