namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class ThreeColumnGraph : BattleGraph
    {
        public ActorNode Left { get; private set; }
        public ActorNode Middle { get; private set; }
        public ActorNode Right { get; private set; }

        protected override void Initialize()
        {
            //map has 3 slots, with one in the middle of the other two
            Left = Context.CreateEntity<ActorNode>();
            Middle = Context.CreateEntity<ActorNode>();
            Right = Context.CreateEntity<ActorNode>();

            var nodeLeft = new ActorGraphNode(Left);
            var nodeRight = new ActorGraphNode(Right);
            var nodeMiddle = new ActorGraphNode(Middle);

            ActorEdge edgeLeft = new ActorEdge(nodeLeft, nodeMiddle);
            ActorEdge edgeLeft2 = new ActorEdge(nodeMiddle, nodeLeft);
            ActorEdge edgeRight = new ActorEdge(nodeMiddle, nodeRight);
            ActorEdge edgeRight2 = new ActorEdge(nodeRight, nodeMiddle);

            Graph.Add(nodeLeft);
            Graph.Add(nodeMiddle);
            Graph.Add(nodeRight);
            Graph.Add(edgeLeft);
            Graph.Add(edgeLeft2);
            Graph.Add(edgeRight);
            Graph.Add(edgeRight2);
        }
    }
}