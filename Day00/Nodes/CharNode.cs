namespace Day00.Nodes
{
    public class CharGenericNode : GenericNode<CharGenericNode, char>
    {
        public CharGenericNode(Coordinate coordinate, char value) : base(coordinate, value)
        {
        }
    }
}
