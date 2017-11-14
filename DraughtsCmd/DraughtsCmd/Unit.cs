using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Unit
    {
        public Player Commander { get; set; }

        public int Xcoord { get; set; }
        public int Ycoord { get; set; }

        public int MoveDir { get; set; }

        public Unit(int x, int y, Player commander)
        {
            Commander = commander;

            Commander.ArmyUnits.Add(this);

            Xcoord = x;
            Ycoord = y;
        }

        public Unit CopyData()
        {
            Unit unit;

            unit = new Unit(Xcoord, Ycoord, Commander);

            return unit;
        }

        public void ConvertToKing(BoardCell cell)
        {
            Unit unit = this;

            Commander.ArmyUnits.Remove(this);

            unit = new King(Xcoord, Ycoord, Commander);
            unit.MoveDir = MoveDir;

            cell.FillCell(unit);
        }

        public void UpdateXY(int x, int y)
        {
            Xcoord = x;
            Ycoord = y;
        }
    }

    class Man : Unit
    {
        public Man(int x, int y, Player commander)
            : base(x, y, commander)
        {
        }
    }

    class King : Unit
    {
        public King(int x, int y, Player commander)
            : base(x, y, commander)
        {
        }
    }
}
