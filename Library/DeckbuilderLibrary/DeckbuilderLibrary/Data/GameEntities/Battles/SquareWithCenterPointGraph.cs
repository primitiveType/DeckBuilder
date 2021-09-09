namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class SquareWithCenterPointGraph : BattleGraph
    {
        public ActorNode TopLeft { get; private set; }
        public ActorNode TopRight { get; private set; }
        public ActorNode BottomLeft { get; private set; }
        public ActorNode BottomRight { get; private set; }
        public ActorNode Middle { get; private set; }

        protected override void Initialize()
        {
            //map has 3 slots, with one in the middle of the other two
            TopLeft = Context.CreateEntity<ActorNode>();
            BottomLeft = Context.CreateEntity<ActorNode>();
            Middle = Context.CreateEntity<ActorNode>();
            TopRight = Context.CreateEntity<ActorNode>();
            BottomRight = Context.CreateEntity<ActorNode>();

            ActorGraphNode nodeTopLeft = new ActorGraphNode(TopLeft);
            ActorGraphNode nodeBottomLeft = new ActorGraphNode(BottomLeft);
            ActorGraphNode nodeMiddle = new ActorGraphNode(Middle);
            ActorGraphNode nodeTopRight = new ActorGraphNode(TopRight);
            ActorGraphNode nodeBottomRight = new ActorGraphNode(BottomRight);

            Graph.Add(nodeTopLeft);
            Graph.Add(nodeMiddle);
            Graph.Add(nodeBottomLeft);
            Graph.Add(nodeTopRight);
            Graph.Add(nodeBottomRight);
            
            ConnectNodes(nodeTopLeft, nodeMiddle);
            ConnectNodes(nodeTopRight, nodeMiddle);
            ConnectNodes(nodeBottomLeft, nodeMiddle);
            ConnectNodes(nodeBottomRight, nodeMiddle);


            ConnectNodes(nodeTopLeft, nodeTopRight);
            ConnectNodes(nodeTopLeft, nodeBottomLeft);
            ConnectNodes(nodeBottomLeft, nodeBottomRight);
            ConnectNodes(nodeBottomRight, nodeTopRight);
        }
    }
}