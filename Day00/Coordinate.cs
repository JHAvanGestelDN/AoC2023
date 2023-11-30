namespace Day00
{
    public sealed class Coordinate
    {
        //2D coordinate
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        //3D coordinate
        public Coordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; }

        protected bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static HashSet<Coordinate> DetermineRange(Coordinate from, Coordinate to)
        {
            HashSet<Coordinate> coordinates = new HashSet<Coordinate>();

            coordinates.Add(from);
            coordinates.Add(to);
            //determine x difference
            int startIndex = from.X < to.X ? from.X : to.X;
            int stopIndex = from.X < to.X ? to.X : from.X;
            for (int i = startIndex; i <= stopIndex; i++)
            {
                coordinates.Add(new Coordinate(i, from.Y));
            }
            startIndex = from.Y < to.Y ? from.Y : to.Y;
            stopIndex = from.Y < to.Y ? to.Y : from.Y;
            for (int i = startIndex; i <= stopIndex; i++)
            {
                coordinates.Add(new Coordinate(from.X, i));
            }



            return coordinates;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Coordinate)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = hashCode * 397 ^ Y;
                hashCode = hashCode * 397 ^ Z;
                return hashCode;
            }
        }
    }
}
