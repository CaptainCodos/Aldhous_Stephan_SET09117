﻿using System;
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
                    Cells[x, y] = new BoardCell(this, x, y);

                    if (y == 0 || y == 7)
                        Cells[x, y].KingsLane = true;
                }
            }

            SetUpPlayer1();
            SetUpPlayer2();
        }

        // setup player 1 army
        public void SetUpPlayer1()
        {
            int val = 0;

            for (int y = 0; y < 8; y++)
            {
                val += 1;

                if (y >= 8 - 3)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        val += 1;

                        if (val % 2 == 0)
                        {
                            Man man = new Man(x, y, m_manager.GetPlayer(0));
                            man.MoveDir = -1;
                            Cells[x, y].FillCell(man);

                            if (Cells[x, y].KingsLane)
                            {
                                Cells[x, y].LaneOwner = m_manager.GetPlayer(0);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                
            }
        }

        // setup player 2 army
        public void SetUpPlayer2()
        {
            int val = 0;

            for (int y = 0; y < 3; y++)
            {
                val += 1;

                for (int x = 0; x < 8; x++)
                {
                    val += 1;

                    if (val % 2 == 0)
                    {
                        Man man = new Man(x, y, m_manager.GetPlayer(1));
                        man.MoveDir = 1;
                        Cells[x, y].FillCell(man);

                        if (Cells[x, y].KingsLane)
                        {
                            Cells[x, y].LaneOwner = m_manager.GetPlayer(1);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        // draw board and all pieces
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

        // draw possible moves on board, each given an index with all pieces
        public void DrawBoard(List<Move> moves)
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
                            string[] tmpS = (string[])Cells[x, y].Content.Clone();

                            for (int k = 0; k < moves.Count; k++)
                            {
                                if (moves[k] is Move)
                                {
                                    if (Cells[x, y] == moves[k].GetEndCell)
                                        tmpS[1] = " " + (k + 1);
                                }
                                else
                                {
                                    Attack aMove = (Attack)moves[k];

                                    if (Cells[x, y] == aMove.GetDefenderCell)
                                        tmpS[1] = " " + (k + 1);
                                }
                            }

                            tmp += tmpS[i];
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
        
        // convert char to int
        public int ConvertYAxisToInt(char letter)
        {
            return Convert.ToInt32(letter) - 97;
        }

        // convert int to char
        public char ConvertYAxisToChar(int val)
        {
            return Convert.ToChar(val + 97);
        }
        
        public int ConvertDistToIdx(int val)
        {
            return val - 1;
        }
    }
}
