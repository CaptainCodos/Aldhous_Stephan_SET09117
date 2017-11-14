using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    struct Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coord operator +(Coord c1, Coord c2)
        {
            return new Coord(c1.X + c2.X, c1.Y + c2.Y);
        }
        public static Coord operator -(Coord c1, Coord c2)
        {
            return new Coord(c1.X - c2.X, c1.Y - c2.Y);
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            bool a = (c1.X == c2.X);
            bool b = (c1.Y == c2.Y);
            return a && b;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            bool a = (c1.X != c2.X);
            bool b = (c1.Y != c2.Y);
            return a || b;
        }
    }
}
