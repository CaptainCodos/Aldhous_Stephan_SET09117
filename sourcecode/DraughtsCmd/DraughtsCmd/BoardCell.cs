using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    /// <summary>
    /// Contains data about the cell on the board
    /// </summary>
    class BoardCell
    {
        // Who current resides in the cell
        public Unit Occupant { get; set; }

        // Content to draw to screen
        public string[] Content { get; set; }

        // Gameboard the cell belongs to
        private GameBoard m_board;

        // Cell Coordinates
        public int XCoord { get; set; }
        public int YCoord { get; set; }

        // Check if cell is in a king lane
        public bool KingsLane { get; set; }
        public Player LaneOwner { get; set; }

        public BoardCell(GameBoard board, int xPos, int yPos)
        {
            m_board = board;
            XCoord = xPos;
            YCoord = yPos;

            Content = new string[3];

            for (int i = 0; i < 3; i++)
            {
                Content[i] = "";

                if (i > 1)
                    Content[i] = "___";
            }
        }

        /// <summary>
        /// Fills the content of the cell depending on the unit filling it
        /// </summary>
        /// <param name="unit"></param>
        public void FillCell(Unit unit)
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
        }

        // Similar to FillCell but empties the cell the unit moved from
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

        // reset cell contents
        public void EmptyCell()
        {
            Occupant = null;

            Content[0] = "";
            Content[1] = "";
            Content[2] = "___";
        }

        // kill the unit in the cell and remove them from the player's army
        public void KillUnit(Unit attacker)
        {
            Occupant.Commander.ArmyUnits.Remove(Occupant);
            attacker.Commander.Kills++;
            EmptyCell();
        }

        public void FillContent(string val)
        {
            Content[1] = " " + val;
        }
        public void EmptyContent()
        {
            FillCell(Occupant);
        }
    }
}
