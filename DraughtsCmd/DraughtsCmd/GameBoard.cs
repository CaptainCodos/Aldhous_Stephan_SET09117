using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class GameBoard
    {
        public BoardCell[,] Cells { get; set; }

        private GameManager m_manager;

        public GameBoard(GameManager manager)
        {
            m_manager = manager;

            Cells = new BoardCell[8, 8];

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Cells[x, y] = new BoardCell(this);
                }
            }
        }

        public void DrawBoard()
        {
            for (int y = -1; y < 8; y++)
            {
                for (int i = 0; i < 3; i++)
                {
                    string line = "";

                    for (int x = -1; x < 8; x++)
                    {
                        string tmp = "";
                        tmp += "|";

                        if (x > -1 && y == -1 && i == 1)
                        {
                            tmp += " " + x;
                        }
                        if (y > -1 && x == -1 && i == 1)
                        {
                            tmp += " " + Convert.ToChar(y + 65);
                        }

                        if (x > -1 && y > -1)
                        {
                            tmp += Cells[x, y].Content[i];
                        }
                        else
                        {
                            if (i > 1)
                                tmp += "___";
                        }

                        line += String.Format("{0,-4}", tmp);
                    }

                    Console.WriteLine(line);
                }
            }
        }

        public int ConvertYAxisToInt(char letter)
        {
            return Convert.ToInt32(letter) - 65;
        }
    }
}
