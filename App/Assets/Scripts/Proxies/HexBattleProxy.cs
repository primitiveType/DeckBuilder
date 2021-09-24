using System.Collections.Generic;
using System.Runtime.Serialization;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public class HexBattleProxy : BattleProxy
{
    private Dictionary<ActorNode, Proxy> NodesByEntity = new Dictionary<ActorNode, Proxy>();

    [SerializeField] private Proxy NodePrefab;
    protected override void OnInitialize()
    {
        base.OnInitialize();
        foreach (var node in GameEntity.Graph.GetNodes())
        {
            var go = Instantiate(NodePrefab, transform);
            NodesByEntity[node.Value] = go;
            
        }

        foreach (var kvp in NodesByEntity)
        {
            kvp.Value.Initialize(kvp.Key);
        }
    }

    public override Proxy GetNodeProxyByEntity(ActorNode entity)
    {
        return NodesByEntity[(ActorNode)entity];
    }

    public void OnDestroy()
    {
        Debug.Log("Battle destroyed.");
    }
}