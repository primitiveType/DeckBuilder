using System;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Terrain;
using UnityEngine;

public class Factory : MonoBehaviourSingleton<Factory>
{
    [SerializeField] private HexBattleProxy BattleProxy;

    [SerializeField] private EnemyActorProxy
        ProxyPrefab;

    [SerializeField] private GameObject
        TerrainProxy;

    [SerializeField] private GameObject
        FireTerrainProxy;

    [SerializeField] private GameObject
        CollectibleProxy;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameContextManager.Instance.Context.Events.EntityCreated += EventsOnEntityCreated;
    }

    public void CreateProxyForEntity(IGameEntity entity)
    {
        GameObject proxy;
        if (entity is Actor)
        {
            proxy = Instantiate(ProxyPrefab, transform).gameObject;
        }
        else if (entity is IBattle)
        {
            proxy = Instantiate(BattleProxy).gameObject;
        }
        else if (entity is BlockedTerrain)
        {
            proxy = Instantiate(TerrainProxy, transform);
        }
        else if (entity is FireTerrain)
        {
            proxy = Instantiate(FireTerrainProxy, transform);
        }
        else if (entity is Collectible)
        {
            proxy = Instantiate(CollectibleProxy, transform);
        }
        else
        {
            return;
        }

        proxy.GetComponent<IProxy>().Initialize(entity);
    }

    private void EventsOnEntityCreated(object sender, EntityCreatedArgs args)
    {
        var entity = args.Entity;
        CreateProxyForEntity(entity);
    }

    private void OnDestroy()
    {
        GameContextManager.Instance.Context.Events.EntityCreated -= EventsOnEntityCreated;
    }
}