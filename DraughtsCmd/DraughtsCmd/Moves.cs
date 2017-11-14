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

        public BoardCell GetEndCell { get { return m_endCell; } }
        public BoardCell GetStartCell { get { return m_startCell; } }

        public Move(BoardCell start, BoardCell end, Player current, Player opponent)
        {
            m_startCell = start;
            m_endCell = end;

            m_current = current;
            m_opponent = opponent;
        }

        public virtual void ExecuteMove(int dist)
        {
            m_endCell.OccupyCell(m_startCell, m_startCell.Occupant);
            m_endCell.Occupant.UpdateXY(m_endCell.XCoord, m_endCell.YCoord);

            if (m_endCell.KingsLane)
            {
                m_endCell.Occupant.ConvertToKing(m_endCell);
            }
        }
    }

    class Attack : Move
    {
        protected BoardCell m_defenderCell;

        public BoardCell GetDefenderCell { get { return m_defenderCell; } }

        public Attack(BoardCell start, BoardCell end, BoardCell defender, Player current, Player opponent)
            : base(start, end, current, opponent)
        {
            m_defenderCell = defender;
        }

        public override void ExecuteMove(int dist)
        {
            m_defenderCell.KillUnit(m_startCell.Occupant);
            base.ExecuteMove(1);
        }
    }

    class KingMove : Move
    {
        public List<Move> Moves { get; set; }

        public KingMove(BoardCell start, BoardCell end, Player current, Player opponent, List<Move> moves)
            : base(start, end, current, opponent)
        {
            Moves = moves;
        }

        public override void ExecuteMove(int dist)
        {
            Moves[dist - 1].ExecuteMove(1);
        }
    }

    class KingAttack : Attack
    {
        public List<Move> Moves { get; set; }

        public KingAttack(BoardCell start, BoardCell end, BoardCell defender, Player current, Player opponent)
            : base(start, end, defender, current, opponent)
        {
            m_defenderCell = defender;
        }

        //public override void ExecuteMove(int dist)
        //{
        //    base.ExecuteMove(1);
        //    m_defenderCell.KillUnit();
        //}
    }
}
