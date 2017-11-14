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

        public Coord UnitCoords { get; set; }

        public int MoveDir { get; set; }

        public Unit(int x, int y, Player commander)
        {
            Commander = commander;

            Commander.ArmyUnits.Add(this);

            UnitCoords = new Coord(x, y);
        }

        public Unit CopyData()
        {
            Unit unit;

            unit = new Unit(UnitCoords.X, UnitCoords.Y, Commander);

            return unit;
        }

        public void ConvertToKing(BoardCell cell)
        {
            Unit unit = this;

            Commander.ArmyUnits.Remove(this);

            unit = new King(UnitCoords.X, UnitCoords.Y, Commander);
            unit.MoveDir = MoveDir;

            cell.FillCell(unit);
        }

        public void UpdateXY(int x, int y)
        {
            UnitCoords = new Coord(x, y);
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
