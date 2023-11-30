namespace Day00.Nodes
{
    public class IntNode : GenericNode<IntNode, int>
    {
        public static readonly List<IntNode> Answers = new List<IntNode>();

        public IntNode(Coordinate coordinate, int value) : base(coordinate, value)
        {
        }
    }
}
