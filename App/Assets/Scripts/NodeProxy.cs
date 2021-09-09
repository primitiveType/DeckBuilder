using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using UnityEngine;

public class NodeProxy : Proxy<ActorNode>
{
    [SerializeField] private EnemyActorProxy
        ProxyPrefab; //I think we can get away with having one enemy prefab that sets itself up based on the data.

    [SerializeField] private PlayerActorProxy
        PlayerProxyPrefab; //I think we can get away with having one enemy prefab that sets itself up based on the data.

    [SerializeField] private GameObject
        m_Visual;

    protected override void OnInitialize()
    {
        if (GameEntity.Actor != null)
        {
            // if (GameEntity.Actor == GameEntity.Context.GetCurrentBattle().Player)
            // {
            //     PlayerActorProxy playerProxy = Instantiate(PlayerProxyPrefab, transform);
            //     playerProxy.Initialize(GameEntity.Actor as PlayerActor);
            // }
            // else
            // {
            var enemyProxy = Instantiate(ProxyPrefab, transform);
            enemyProxy.Initialize((Actor)GameEntity.Actor);
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