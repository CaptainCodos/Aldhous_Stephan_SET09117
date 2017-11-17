using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class Game
    {
        private GameManager m_manager; // manager for this game

        public bool GameEnded { get; set; } // game ending flag
        public int Winner { get; set; } // value for announcement to figure out who won the game

        public Game(bool hasAI)
        {
            m_manager = new GameManager(this, hasAI);
        }

        public GameManager GetManager()
        {
            return m_manager;
        }

        // Checks if game is complete
        public void CheckIfComplete()
        {
            if (m_manager.GetPlayer(0).Units <= 0 || m_manager.GetPlayer(1).Units <= 0)
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
            else
            {
                GameEnded = false;
            }
        }
    }
}
