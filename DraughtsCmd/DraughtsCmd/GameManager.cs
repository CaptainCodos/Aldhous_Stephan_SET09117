using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class GameManager
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public GameBoard Board { get; set; }

        private Game m_game;

        public GameManager(Game game)
        {
            m_game = game;

            Board = new GameBoard(this);
        }
    }
}
