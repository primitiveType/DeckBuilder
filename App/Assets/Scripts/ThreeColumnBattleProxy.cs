using System.Collections.Generic;
using System.Xml;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;

public class ThreeColumnBattleProxy : BattleProxy
{
    [SerializeField] NodeProxy LeftNode;
    [SerializeField] NodeProxy MiddleNode;
    [SerializeField] NodeProxy RightNode;

    private Dictionary<GameEntity, NodeProxy> NodesByEntity = new Dictionary<GameEntity, NodeProxy>();
    private ThreeColumnGraph Graph => GameEntity.Graph as ThreeColumnGraph;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        NodesByEntity.Add(Graph.Left, LeftNode);
        NodesByEntity.Add(Graph.Middle, MiddleNode);
        NodesByEntity.Add(Graph.Right, RightNode);
        LeftNode.Initialize(Graph.Left);
        MiddleNode.Initialize(Graph.Middle);
        RightNode.Initialize(Graph.Right);
    }

    public override NodeProxy GetNodeProxyByEntity(GameEntity entity)
    {
        return NodesByEntity[entity];
    }
}