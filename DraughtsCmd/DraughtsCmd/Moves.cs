using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Move
    {
        protected BoardCell m_startCell;
        protected BoardCell m_endCell;

        protected Player m_current;
        protected Player m_opponent;

        public Move(BoardCell start, BoardCell end, Player current, Player opponent)
        {
            m_startCell = start;
            m_endCell = end;

            m_current = current;
            m_opponent = opponent;
        }

        public virtual void ExecuteMove()
        {
            m_endCell.OccupyCell(m_startCell, m_startCell.Occupant);
        }
    }

    class Attack : Move
    {
        private BoardCell m_defenderCell;

        public Attack(BoardCell start, BoardCell end, BoardCell defender, Player current, Player opponent)
            : base(start, end, current, opponent)
        {
            m_defenderCell = defender;
        }

        public override void ExecuteMove()
        {
            base.ExecuteMove();
            m_defenderCell.KillUnit();
        }
    }
}
