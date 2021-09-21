using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Terrain;
using UnityEngine;

public class NodeProxy : Proxy<ActorNode>
{
    [SerializeField] private EnemyActorProxy
        ProxyPrefab; //I think we can get away with having one enemy prefab that sets itself up based on the data.

    [SerializeField] private PlayerActorProxy
        PlayerProxyPrefab; //I think we can get away with having one enemy prefab that sets itself up based on the data.
    [SerializeField] private GameObject
        TerrainProxy; //I think we can get away with having one enemy prefab that sets itself up based on the data.

    [SerializeField] private GameObject
        m_Visual;

    protected override void OnInitialize()
    {
        var worldCoord = GameEntity.Graph.Grid.AxialToPoint(GameEntity.Coordinate.ToAxial());
        transform.localPosition = new Vector3(worldCoord.x, worldCoord.y, 0);
        foreach (var entityRef in GameEntity.CurrentEntities)
        {
            var entity = entityRef.Entity;
            if (entity is Actor)
            {
                var enemyProxy = Instantiate(ProxyPrefab, transform);
                enemyProxy.Initialize((Actor)GameEntity.GetActor());
            }
            else if (entity is BlockedTerrain)
            {
                var terrainProxy = Instantiate(TerrainProxy, transform);
                // terrainProxy.Initialize((Actor)GameEntity.GetActor());
            }
        }
        if (GameEntity.GetActor() != null)
        {
            // if (GameEntity.Actor == GameEntity.Context.GetCurrentBattle().Player)
            // {
            //     PlayerActorProxy playerProxy = Instantiate(PlayerProxyPrefab, transform);
            //     playerProxy.Initialize(GameEntity.Actor as PlayerActor);
            // }
            // else
            // {
            
            // }
        }
    }


    private Material MyMaterial { get; set; }

    public GameObject Visual => m_Visual;

    private void Awake()
    {
        MyMaterial = GetComponentInChildren<Renderer>().material;
    }
}