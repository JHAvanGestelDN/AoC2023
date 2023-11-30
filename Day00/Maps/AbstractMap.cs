using Day00.Nodes;
namespace Day00.Maps
{
    public abstract class AbstractMap<T, V> where T : GenericNode<T, V>
    {
        protected AbstractMap(int x, int y)
        {
            Map = new T[x, y];
        }

        public T[,] Map { get; }

        public void Print()
        {
            for (var x = 0; x < Map.GetLength(1); x++)
            {
                for (var y = 0; y < Map.GetLength(0); y++)
                {
                    Console.Write(Map[y, x] != null ? Map[y, x].Value : '.');
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static void AddNeighbours(T[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].AddNeighboursDiagonal(map);
                }
            }
        }

        public void FillMap(Coordinate coordinate, T t)
        {
            Map[coordinate.X, coordinate.Y] = t;
        }
    }
}
