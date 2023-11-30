namespace Day00.Nodes
{
    public class GenericNode<T, V>
    {
        protected GenericNode(Coordinate coordinate, V value)
        {
            Coordinate = coordinate;
            Value = value;
        }

        public Coordinate Coordinate { get; }
        public List<T> Neighbours { get; } = new List<T>();
        public V Value { get; set; }

        //Pathfinding attributes
        public GenericNode<T, V> previous { get; set; }
        public int cost { get; set; } = Int32.MaxValue;
        public int heuristic { get; set; } = Int32.MaxValue;
    

        public int CalculateHeuristic(GenericNode<T, V> goal)
        {
            return Math.Abs(Coordinate.X - goal.Coordinate.X) + Math.Abs(Coordinate.Y - goal.Coordinate.Y);
        }

        public void AddNeighbours(T[,] map)
        {
            if (Coordinate.X > 0)
                Neighbours.Add(map[Coordinate.X - 1, Coordinate.Y]);
            if (Coordinate.X + 1 < map.GetLength(0))
                Neighbours.Add(map[Coordinate.X + 1, Coordinate.Y]);
            if (Coordinate.Y > 0)
                Neighbours.Add(map[Coordinate.X, Coordinate.Y - 1]);
            if (Coordinate.Y + 1 < map.GetLength(1))
                Neighbours.Add(map[Coordinate.X, Coordinate.Y + 1]);
        }

        public void AddNeighboursDiagonal(T[,] map)
        {
            AddNeighbours(map);

            //top left 
            if (Coordinate.X > 0 && Coordinate.Y > 0)
                Neighbours.Add(map[Coordinate.X - 1, Coordinate.Y - 1]);
            //top right 
            if (Coordinate.X > 0 && Coordinate.Y + 1 < map.GetLength(1))
                Neighbours.Add(map[Coordinate.X - 1, Coordinate.Y + 1]);

            //bottom left 
            if (Coordinate.X + 1 < map.GetLength(0) && Coordinate.Y > 0)
                Neighbours.Add(map[Coordinate.X + 1, Coordinate.Y - 1]);
            //bottom right 
            if (Coordinate.X + 1 < map.GetLength(0) && Coordinate.Y + 1 < map.GetLength(1))
                Neighbours.Add(map[Coordinate.X + 1, Coordinate.Y + 1]);
        }

        protected bool Equals(GenericNode<T, V> other)
        {
            return Coordinate.Equals(other.Coordinate);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((GenericNode<T, V>)obj);
        }
        public override int GetHashCode()
        {
            return Coordinate.GetHashCode();
        }
        private sealed class CoordinateEqualityComparer : IEqualityComparer<GenericNode<T, V>>
        {
            public bool Equals(GenericNode<T, V> x, GenericNode<T, V> y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (ReferenceEquals(x, null))
                    return false;
                if (ReferenceEquals(y, null))
                    return false;
                if (x.GetType() != y.GetType())
                    return false;
                return x.Coordinate.Equals(y.Coordinate);
            }
            public int GetHashCode(GenericNode<T, V> obj)
            {
                return obj.Coordinate.GetHashCode();
            }
        }
        public static IEqualityComparer<GenericNode<T, V>> coordinateComparer { get; } = new CoordinateEqualityComparer();
    }
}