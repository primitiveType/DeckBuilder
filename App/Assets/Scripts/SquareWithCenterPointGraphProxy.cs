using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;

public class SquareWithCenterPointGraphProxy : BattleProxy
{
    [SerializeField] NodeProxy TopLeftNode;
    [SerializeField] NodeProxy MiddleNode;
    [SerializeField] NodeProxy TopRightNode;

    [SerializeField] NodeProxy BottomLeftNode;
    [SerializeField] NodeProxy BottomRightNode;

    private Dictionary<GameEntity, NodeProxy> NodesByEntity = new Dictionary<GameEntity, NodeProxy>();
    private SquareWithCenterPointGraph Graph => GameEntity.Graph as SquareWithCenterPointGraph;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        NodesByEntity.Add(Graph.TopLeft, TopLeftNode);
        NodesByEntity.Add(Graph.TopRight, TopRightNode);
        NodesByEntity.Add(Graph.BottomLeft, BottomLeftNode);
        NodesByEntity.Add(Graph.BottomRight, BottomRightNode);
        NodesByEntity.Add(Graph.Middle, MiddleNode);

        TopLeftNode.Initialize(Graph.TopLeft);
        TopRightNode.Initialize(Graph.TopRight);
        BottomLeftNode.Initialize(Graph.BottomLeft);
        BottomRightNode.Initialize(Graph.BottomRight);
        MiddleNode.Initialize(Graph.Middle);
        
    }

    public override NodeProxy GetNodeProxyByEntity(GameEntity entity)
    {
        return NodesByEntity[entity];
    }
}