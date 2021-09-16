namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    // public class SquareWithCenterPointGraph : BattleGraph
    // {
    //     public ActorNode TopLeft { get; private set; }
    //     public ActorNode TopRight { get; private set; }
    //     public ActorNode BottomLeft { get; private set; }
    //     public ActorNode BottomRight { get; private set; }
    //     public ActorNode Middle { get; private set; }
    //
    //     protected override void Initialize()
    //     {
    //         //map has 3 slots, with one in the middle of the other two
    //         TopLeft = Context.CreateEntity<ActorNode>();
    //         BottomLeft = Context.CreateEntity<ActorNode>();
    //         Middle = Context.CreateEntity<ActorNode>();
    //         TopRight = Context.CreateEntity<ActorNode>();
    //         BottomRight = Context.CreateEntity<ActorNode>();
    //
    //         //nonsense values
    //         HexGraphNode nodeTopLeft = new HexGraphNode(TopLeft, 0, 0, 0);
    //         HexGraphNode nodeBottomLeft = new HexGraphNode(BottomLeft, 0, 1, 0);
    //         HexGraphNode nodeMiddle = new HexGraphNode(Middle, 0, 0, 1);
    //         HexGraphNode nodeTopRight = new HexGraphNode(TopRight, 0, 2, 0);
    //         HexGraphNode nodeBottomRight = new HexGraphNode(BottomRight, 0, 3, 0);
    //
    //         Graph.Add(nodeTopLeft);
    //         Graph.Add(nodeMiddle);
    //         Graph.Add(nodeBottomLeft);
    //         Graph.Add(nodeTopRight);
    //         Graph.Add(nodeBottomRight);
    //
    //         ConnectNodes(nodeTopLeft, nodeMiddle);
    //         ConnectNodes(nodeTopRight, nodeMiddle);
    //         ConnectNodes(nodeBottomLeft, nodeMiddle);
    //         ConnectNodes(nodeBottomRight, nodeMiddle);
    //
    //
    //         ConnectNodes(nodeTopLeft, nodeTopRight);
    //         ConnectNodes(nodeTopLeft, nodeBottomLeft);
    //         ConnectNodes(nodeBottomLeft, nodeBottomRight);
    //         ConnectNodes(nodeBottomRight, nodeTopRight);
    //     }
    // }
}