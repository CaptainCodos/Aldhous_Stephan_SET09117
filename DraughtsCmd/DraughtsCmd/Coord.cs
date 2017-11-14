using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    /// <summary>
    /// Coordinate struct for use in checking board cells
    /// </summary>
    struct Coord
    {
        public int X { get; set; } // X Component of the coordinate
        public int Y { get; set; } // Y Component of the coordinate

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
