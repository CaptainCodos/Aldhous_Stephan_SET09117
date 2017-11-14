using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Game
    {
        private GameManager m_manager;

        public int TurnsSkipped { get; set; }

        public bool GameEnded { get; set; }
        public int Winner { get; set; }

        public Game(bool hasAI)
        {
            m_manager = new GameManager(this, hasAI);
            TurnsSkipped = 0;
        }

        public GameManager GetManager()
        {
            return m_manager;
        }

        public void CheckIfComplete()
        {
            if (TurnsSkipped > 1 || m_manager.GetPlayer(0).Units <= 0 || m_manager.GetPlayer(1).Units <= 0)
            {
                GameEnded = true;

                if (m_manager.GetPlayer(0).Units <= 0)
                {
                    Winner = 1;
                }
                else if (m_manager.GetPlayer(1).Units <= 0)
                {
                    Winner = 0;
                }
            }
        }
    }
}
