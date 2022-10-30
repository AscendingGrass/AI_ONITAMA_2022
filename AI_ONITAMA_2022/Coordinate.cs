
namespace AI_ONITAMA_2022
{
    public struct Coordinate
    {
        public int X, Y;

        #region constructors
        public Coordinate(int x = 0 , int y = 0) => 
            (X, Y) = (x, y);
        #endregion

        #region operator overloads
        public static Coordinate operator+(Coordinate c1, Coordinate c2) =>
            new Coordinate(c1.X + c2.X, c1.Y + c2.Y);
        public static Coordinate operator-(Coordinate c1, Coordinate c2) =>
            new Coordinate(c1.X - c2.X, c1.Y - c2.Y);
        public static Coordinate operator *(Coordinate c1, Coordinate c2) =>
            new Coordinate(c1.X * c2.X, c1.Y * c2.Y);
        public static Coordinate operator *(Coordinate c1, int mult) =>
            new Coordinate(c1.X * mult, c1.Y * mult);
        public static Coordinate operator *(int mult, Coordinate c1) =>
            new Coordinate(c1.X * mult, c1.Y * mult);
        public static Coordinate operator /(Coordinate c1, Coordinate c2) =>
            new Coordinate(c1.X / c2.X, c1.Y / c2.Y);
        public static Coordinate operator /(Coordinate c1, int div) =>
            new Coordinate(c1.X / div, c1.Y / div);

        #endregion

        public override string ToString() =>
            $"Coord({X},{Y})";

        public void Deconstruct(out double x, out double y) =>
            (x, y) = (X, Y);
    }

}

