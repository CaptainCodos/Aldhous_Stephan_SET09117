using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Unit
    {
        // Player in charge of this unit
        public Player Commander { get; set; }

        // Unit's location on the game board
        public Coord UnitCoords { get; set; }

        // Direction the unit moves (matters little as king)
        public int MoveDir { get; set; }

        // Flag to check if the unit just got converted to a king
        public bool Converted { get; set; }

        public Unit(int x, int y, Player commander)
        {
            Commander = commander;

            Commander.ArmyUnits.Add(this);

            UnitCoords = new Coord(x, y);

            Converted = false;
        }
        
        public Unit CopyData()
        {
            Unit unit;

            unit = new Unit(UnitCoords.X, UnitCoords.Y, Commander);

            return unit;
        }

        // Turn the unit into a king by removing it from player's army and replacing it with a king in the player's army
        public void ConvertToKing(BoardCell cell)
        {
            Unit unit = this;

            Commander.ArmyUnits.Remove(this);

            unit = new King(UnitCoords.X, UnitCoords.Y, Commander);
            unit.MoveDir = MoveDir;

            cell.FillCell(unit);
        }

        // Updates the unit's coordinates
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
