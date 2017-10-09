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

        public Game()
        {
            m_manager = new GameManager(this);
        }
    }
}
