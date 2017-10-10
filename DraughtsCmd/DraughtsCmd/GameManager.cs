using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class GameManager
    {
        public Player[] Players { get; set; }

        public GameBoard Board { get; set; }

        public int Turn { get; set; }
        public Player CurrPlayer { get; set; }

        private Game m_game;

        public GameManager(Game game)
        {
            m_game = game;

            Players = new Player[2];

            Players[0] = new Player("O");
            Players[1] = new Player("X");

            Board = new GameBoard(this);

            Turn = 0;

            CurrPlayer = Players[Turn % 2];
        }

        public Player GetPlayer(int player)
        {
            return Players[player];
        }

        public void ConductTurn()
        {


            Turn += 1;
            CurrPlayer = Players[Turn % 2];
        }
    }
}
