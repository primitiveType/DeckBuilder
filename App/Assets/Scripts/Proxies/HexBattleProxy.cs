using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public class HexBattleProxy : BattleProxy
{
    private Dictionary<ActorNode, NodeProxy> NodesByEntity = new Dictionary<ActorNode, NodeProxy>();

    [SerializeField] private NodeProxy NodePrefab;
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

    public override NodeProxy GetNodeProxyByEntity(ActorNode entity)
    {
        return NodesByEntity[(ActorNode)entity];
    }
}