using System.Text;
using Day00.Nodes;
namespace Day00.Maps
{
    public class CharMap : AbstractMap<CharGenericNode, char>
    {
        public CharMap(int x, int y) : base(x, y)
        {
        }

        public int CountValues()
        {
            var sum = 0;
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] != null && Map[i, j].Value == '#')
                        sum++;
                }
            }

            return sum;
        }
        public void FillNeighbours()
        {

            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    CharGenericNode node = Map[i, j];
                    int value = DetermineAsciiValue(node.Value.ToString());
                    if (node.Value == 'S')
                        value = DetermineAsciiValue("a");
                    if (node.Value == 'E')
                        value = DetermineAsciiValue("z");

                    //get the neighbours
                    var nodeUp = i - 1 >= 0 ? Map[i - 1, j] : null;
                    var nodeDown = i + 1 < Map.GetLength(0) ? Map[i + 1, j] : null;
                    var nodeLeft = j - 1 >= 0 ? Map[i, j - 1] : null;
                    var nodeRight = j + 1 < Map.GetLength(1) ? Map[i, j + 1] : null;

                    if (IsNeighbour(nodeUp, value))
                        node.Neighbours.Add(nodeUp);
                    if (IsNeighbour(nodeDown, value))
                        node.Neighbours.Add(nodeDown);
                    if (IsNeighbour(nodeLeft, value))
                        node.Neighbours.Add(nodeLeft);
                    if (IsNeighbour(nodeRight, value))
                        node.Neighbours.Add(nodeRight);
                }
            }
        }

        private bool IsNeighbour(CharGenericNode node, int value)
        {
            if (node == null)
                return false;
            if (node.Value == 'E')
                return true;
            return DetermineAsciiValue(node.Value.ToString()) - value <= 1;


        }
        public static int DetermineAsciiValue(string s)
        {
            return Encoding.ASCII.GetBytes(s)[0];
        }
        public List<CharGenericNode> GetLowestElevationNodes()
        {
            var result = new List<CharGenericNode>();
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] != null && Map[i, j].Value == 'a' || Map[i, j].Value == 'S')
                        result.Add(Map[i, j]);
                }
            }
            return result;
        }
        public void ResetCost()
        {
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j].cost = Int32.MaxValue;
                }
            }
        }
    }
}
