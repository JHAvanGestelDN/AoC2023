using Day00.Nodes;
namespace Day00.Maps
{
    public class IntMap : AbstractMap<IntNode, int>
    {
        public IntMap(int x, int y) : base(x, y)
        {
        }

        public static IntMap CreateMap(IReadOnlyList<string> input)
        {
            var map = new IntMap(input.Count, input[0].Length);

            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    var charValue = input[i][j].ToString();
                    map.Map[i, j] = new IntNode(new Coordinate(i, j), int.Parse(charValue));
                }
            }

            return map;
        }

        public void AddNeighbours()
        {
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j].AddNeighbours(Map);
                }
            }
        }
        //get neighbour based on direction. if out of bounds, return null
        public IntNode GetNeighbour(IntNode node, Direction direction)
        {
            var x = node.Coordinate.X;
            var y = node.Coordinate.Y;

            return direction switch
            {
            Direction.Up => x - 1 < 0 ? null : Map[x - 1, y],
            Direction.Down => x + 1 >= Map.GetLength(0) ? null : Map[x + 1, y],
            Direction.Left => y - 1 < 0 ? null : Map[x, y - 1],
            Direction.Right => y + 1 >= Map.GetLength(1) ? null : Map[x, y + 1],
            _ => null
            };
        }

        private bool IsVisible(IntNode node)
        {
            var result = false;
            //iterate over all directions and check if all neighbours in a row or colomn have a value lower than this node
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var neighbour = GetNeighbour(node, direction);
                //if neighbour is null the node is on the edge of the map and is visible
                if (neighbour == null)
                {
                    result = true;
                    break;
                }
                //if neighbour is lower than current node, it is visible and we need to keep checking based on the direction
                if (neighbour.Value < node.Value)
                {
                    result = true;
                    while (neighbour != null)
                    {
                        if (neighbour.Value >= node.Value)
                        {
                            result = false;
                            break;
                        }
                        neighbour = GetNeighbour(neighbour, direction);
                    }
                    if (result)
                        break;
                }
                //if neighbour is higher than current node, it is not visible and we can stop checking this direction but need to check the other directions
                else
                {
                    result = false;
                }
            }
            return result;
        }


        private int CalculateScenicScore(IntNode node)
        {
            var score = 1;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {

                var neighbour = GetNeighbour(node, direction);
                if (neighbour == null)
                    return 0;

                //iterate over all neighbours in a row or colomn and add 1 to the score for each neighbour that is lower than the current node
                var localScore = 0;
                while (neighbour != null)
                {
                    localScore++;
                    if (neighbour.Value >= node.Value)
                    {
                        break;
                    }
                    neighbour = GetNeighbour(neighbour, direction);
                }
                score *= localScore;

            }


            return score;
        }
        public int DetermineNodeWithHighestScore()
        {
            var max = 0;
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    max = Math.Max(max, CalculateScenicScore(Map[i, j]));
                }
            }
            return max;

        }
        public int CountVisible()
        {
            var count = 0;
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    if (IsVisible(Map[i, j]))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
