using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Move
    {
        protected BoardCell m_startCell; // cell moved from
        protected BoardCell m_endCell; // cell moved to

        protected Player m_current; // current player
        protected Player m_opponent; // opposing player

        public BoardCell GetEndCell { get { return m_endCell; } }
        public BoardCell GetStartCell { get { return m_startCell; } }

        public Move(BoardCell start, BoardCell end, Player current, Player opponent)
        {
            m_startCell = start;
            m_endCell = end;

            m_current = current;
            m_opponent = opponent;
        }

        // move the unit from start to end and convert unit to a king if they land in a king lane
        public virtual void ExecuteMove(int dist)
        {
            m_endCell.OccupyCell(m_startCell, m_startCell.Occupant);
            m_endCell.Occupant.UpdateXY(m_endCell.XCoord, m_endCell.YCoord);

            if (m_endCell.KingsLane && m_endCell.Occupant is Man)
            {
                m_endCell.Occupant.ConvertToKing(m_endCell);
                m_endCell.Occupant.Converted = true;
            }
        }
    }

    class Attack : Move
    {
        protected BoardCell m_defenderCell; // cell being attacked

        public BoardCell GetDefenderCell { get { return m_defenderCell; } }

        public Attack(BoardCell start, BoardCell end, BoardCell defender, Player current, Player opponent)
            : base(start, end, current, opponent)
        {
            m_defenderCell = defender;
        }

        // kill the unit in the cell defending and move the unit
        public override void ExecuteMove(int dist)
        {
            m_defenderCell.KillUnit(m_startCell.Occupant);
            base.ExecuteMove(1);
        }
    }
}
