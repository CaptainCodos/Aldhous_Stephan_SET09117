using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class BoardCell
    {
        public Unit Occupant { get; set; }

        public string[] Content { get; set; }

        private GameBoard m_board;

        public BoardCell(GameBoard board)
        {
            m_board = board;

            Content = new string[3];

            for (int i = 0; i < 3; i++)
            {
                Content[i] = "";

                if (i > 1)
                    Content[i] = "___";
            }
        }

        public void OccupyCell(BoardCell prevCell, Unit unit)
        {
            Occupant = unit;

            if (unit is Man)
            {
                Content[0] = "";
                Content[1] = " " + unit.Commander.BoardSign;
                Content[2] = "___";
            }
            else
            {
                Content[0] = "";
                Content[1] = "(" + unit.Commander.BoardSign + ")";
                Content[2] = "___";
            }

            prevCell.EmptyCell();
        }

        public void EmptyCell()
        {
            Occupant = null;

            Content[0] = "";
            Content[1] = "";
            Content[2] = "___";
        }

        public void KillUnit()
        {
            Occupant.Commander.ArmyUnits.Remove(Occupant);

            EmptyCell();
        }
    }
}
