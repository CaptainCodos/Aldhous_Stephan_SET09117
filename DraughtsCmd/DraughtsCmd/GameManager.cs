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

        public List<Move> Attacks { get; set; }
        public List<Move> Moves { get; set; }
        public List<BoardCell> CellsAble { get; set; }

        private Game m_game;

        public GameManager(Game game)
        {
            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();

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

        public void ConductTurn(Move move)
        {
            move.ExecuteMove(1);

            BoardCell curr = move.GetEndCell;
            CellsAble.Add(curr);

            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();

            if (move is Attack)
            {
                AddPossibleAttacks(curr.XCoord, curr.YCoord);
            }
        }

        public void CancelTurn()
        {
            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();
        }

        public void FinishTurn()
        {
            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();

            Turn += 1;
            CurrPlayer = Players[Turn % 2];
        }

        public void GetUnits()
        {
            for (int i = 0; i < CurrPlayer.ArmyUnits.Count; i++)
            {
                AddPossibleAttacks(CurrPlayer.ArmyUnits[i].Xcoord, CurrPlayer.ArmyUnits[i].Ycoord);
            }

            if (Attacks.Count < 1)
            {
                for (int i = 0; i < CurrPlayer.ArmyUnits.Count; i++)
                {
                    AddPossibleMoves(CurrPlayer.ArmyUnits[i].Xcoord, CurrPlayer.ArmyUnits[i].Ycoord);
                }
            }
        }

        public List<Move> GetAttacks()
        {
            return null;
        }

        public List<Move> GetMovesOfCell(BoardCell cell)
        {
            List<Move> cellMoves = new List<Move>();

            for (int i = 0; i < Attacks.Count; i++)
            {
                if (Attacks[i].GetStartCell == cell)
                {
                    cellMoves.Add(Attacks[i]);
                }
            }

            if (Attacks.Count <= 0)
            {
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].GetStartCell == cell)
                    {
                        cellMoves.Add(Moves[i]);
                    }
                }
            }

            return cellMoves;
        }

        public void AddPossibleAttacks(int x, int y)
        {
            if (Board.Cells[x, y].Occupant is Man)
            {
                AddAttacksMan(Board.Cells[x, y].Occupant);
            }
            else
            {
                AddAttacksKing(Board.Cells[x, y].Occupant);
            }
        }

        public void AddAttacksMan(Unit man)
        {
            int yDir = man.MoveDir;
            int manX = man.Xcoord;
            int manY = man.Ycoord;

            BoardCell curr = Board.Cells[manX, manY];

            if (IsCoordsOnBoard(manX + 1, manY + yDir))
            {
                if (Board.Cells[manX + 1, manY + yDir].Occupant != null && Board.Cells[manX + 1, manY + yDir].Occupant.Commander != CurrPlayer)
                {
                    BoardCell defender = Board.Cells[manX + 1, manY + yDir];

                    if (IsCoordsOnBoard(manX + 2, manY + yDir + yDir) && Board.Cells[manX + 2, manY + yDir + yDir].Occupant == null)
                    {
                        BoardCell finish = Board.Cells[manX + 2, manY + yDir + yDir];
                        Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

                        if (!CellsAble.Contains(curr))
                        {
                            CellsAble.Add(curr);
                        }
                    }
                }
            }

            if (IsCoordsOnBoard(manX - 1, manY + yDir))
            {
                if (Board.Cells[manX - 1, manY + yDir].Occupant != null && Board.Cells[manX - 1, manY + yDir].Occupant.Commander != CurrPlayer)
                {
                    BoardCell defender = Board.Cells[manX - 1, manY + yDir];

                    if (IsCoordsOnBoard(manX - 2, manY + yDir + yDir) && Board.Cells[manX - 2, manY + yDir + yDir].Occupant == null)
                    {
                        BoardCell finish = Board.Cells[manX - 2, manY + yDir + yDir];
                        Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

                        if (!CellsAble.Contains(curr))
                        {
                            CellsAble.Add(curr);
                        }
                    }
                }
            }
        }

        //public void AddAttacksKing(Unit king)
        //{
        //    int yDir = king.MoveDir;
        //    int kX = king.Xcoord;
        //    int kY = king.Ycoord;

        //    BoardCell curr = Board.Cells[kX, kY];

        //    int[] currCoords = new int[2];
        //    currCoords[0] = kX;
        //    currCoords[1] = kY;

        //    int[][] dirs = new int[4][];

        //    dirs[0] = new int[2] { 1, yDir };
        //    dirs[1] = new int[2] { -1, yDir };
        //    dirs[2] = new int[2] { 1, -yDir };
        //    dirs[3] = new int[2] { -1, -yDir };

        //    int i = 0;

        //    while(IsCoordsOnBoard(currCoords[0], currCoords[1]) && i < dirs.Length)
        //    {
        //        currCoords[0] += dirs[i][0];
        //        currCoords[1] += dirs[i][1];

        //        if (IsCoordsOnBoard(currCoords[0], currCoords[1]))
        //        {
        //            if (Board.Cells[currCoords[0], currCoords[1]].Occupant != null && Board.Cells[currCoords[0], currCoords[1]].Occupant.Commander != CurrPlayer)
        //            {
        //                BoardCell defender = Board.Cells[currCoords[0], currCoords[1]];

        //                if (IsCoordsOnBoard(currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]))
        //                {
        //                    BoardCell finish = Board.Cells[currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]];

        //                    Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

        //                    if (!CellsAble.Contains(curr))
        //                    {
        //                        CellsAble.Add(curr);
        //                    }

        //                    i++;

        //                    currCoords[0] = kX;
        //                    currCoords[1] = kY;
        //                }
        //            }
        //        }

        //        if (!IsCoordsOnBoard(currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]))
        //        {
        //            i++;

        //            currCoords[0] = kX;
        //            currCoords[1] = kY;
        //        }
        //    }
        //}

        public void AddAttacksKing(Unit king)
        {
            int yDir = king.MoveDir;
            int kX = king.Xcoord;
            int kY = king.Ycoord;

            BoardCell curr = Board.Cells[kX, kY];

            int[] currCoords = new int[2];
            currCoords[0] = kX;
            currCoords[1] = kY;

            int[][] dirs = new int[4][];

            dirs[0] = new int[2] { 1, yDir };
            dirs[1] = new int[2] { -1, yDir };
            dirs[2] = new int[2] { 1, -yDir };
            dirs[3] = new int[2] { -1, -yDir };

            for (int i = 0; i < dirs.Length; i++)
            {
                int cX = currCoords[0] + dirs[i][0];
                int cY = currCoords[1] + dirs[i][1];

                if (IsCoordsOnBoard(cX, cY))
                {
                    if (Board.Cells[cX, cY].Occupant != null && Board.Cells[cX, cY].Occupant.Commander != CurrPlayer)
                    {
                        BoardCell defender = Board.Cells[cX, cY];

                        if (IsCoordsOnBoard(cX + dirs[i][0], cY + dirs[i][1]))
                        {
                            if (Board.Cells[cX + dirs[i][0], cY + dirs[i][1]].Occupant == null)
                            {
                                BoardCell finish = Board.Cells[cX + dirs[i][0], cY + dirs[i][1]];

                                Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

                                if (!CellsAble.Contains(curr))
                                {
                                    CellsAble.Add(curr);
                                }
                            }
                        }
                    }
                }
            }

            //int i = 0;

            //while (IsCoordsOnBoard(currCoords[0], currCoords[1]) && i < dirs.Length)
            //{
            //    currCoords[0] += dirs[i][0];
            //    currCoords[1] += dirs[i][1];

            //    if (IsCoordsOnBoard(currCoords[0], currCoords[1]))
            //    {
            //        if (Board.Cells[currCoords[0], currCoords[1]].Occupant != null && Board.Cells[currCoords[0], currCoords[1]].Occupant.Commander != CurrPlayer)
            //        {
            //            BoardCell defender = Board.Cells[currCoords[0], currCoords[1]];

            //            if (IsCoordsOnBoard(currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]))
            //            {
            //                BoardCell finish = Board.Cells[currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]];

            //                Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

            //                if (!CellsAble.Contains(curr))
            //                {
            //                    CellsAble.Add(curr);
            //                }

            //                i++;

            //                currCoords[0] = kX;
            //                currCoords[1] = kY;
            //            }
            //        }
            //    }

            //    if (!IsCoordsOnBoard(currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]))
            //    {
            //        i++;

            //        currCoords[0] = kX;
            //        currCoords[1] = kY;
            //    }
            //}
        }

        public void AddPossibleMoves(int x, int y)
        {
            if (Board.Cells[x, y].Occupant is Man)
            {
                AddMovesMan(Board.Cells[x, y].Occupant);
            }
            else
            {
                AddMovesKing(Board.Cells[x, y].Occupant);
            }
        }

        public void AddMovesMan(Unit man)
        {
            int yDir = man.MoveDir;
            int manX = man.Xcoord;
            int manY = man.Ycoord;

            BoardCell curr = Board.Cells[manX, manY];

            if (IsCoordsOnBoard(manX + 1, manY + yDir))
            {
                if (Board.Cells[manX + 1, manY + yDir].Occupant == null)
                {
                    BoardCell finish = Board.Cells[manX + 1, manY + yDir];
                    int opponentIdx = (Turn + 1) % 2;
                    Player opponent = GetPlayer(opponentIdx);
                    Moves.Add(new Move(curr, finish, CurrPlayer, opponent));

                    if (!CellsAble.Contains(curr))
                    {
                        CellsAble.Add(curr);
                    }
                }
            }

            if (IsCoordsOnBoard(manX - 1, manY + yDir))
            {
                if (Board.Cells[manX - 1, manY + yDir].Occupant == null)
                {
                    BoardCell finish = Board.Cells[manX - 1, manY + yDir];

                    int opponentIdx = (Turn + 1) % 2;
                    Player opponent = GetPlayer(opponentIdx);
                    Moves.Add(new Move(curr, finish, CurrPlayer, opponent));

                    if (!CellsAble.Contains(curr))
                    {
                        CellsAble.Add(curr);
                    }
                }
            }
        }

        //public void AddMovesKing(Unit king)
        //{
        //    int yDir = king.MoveDir;
        //    int kX = king.Xcoord;
        //    int kY = king.Ycoord;

        //    BoardCell curr = Board.Cells[kX, kY];

        //    int[] currCoords = new int[2];
        //    currCoords[0] = kX;
        //    currCoords[1] = kY;

        //    int[][] dirs = new int[4][];

        //    dirs[0] = new int[2] { 1, yDir };
        //    dirs[1] = new int[2] { -1, yDir };
        //    dirs[2] = new int[2] { 1, -yDir };
        //    dirs[3] = new int[2] { -1, -yDir };

        //    int i = 0;

        //    List<Move> moves = new List<Move>();

        //    while (IsCoordsOnBoard(currCoords[0], currCoords[1]) && i < dirs.Length)
        //    {
        //        currCoords[0] += dirs[i][0];
        //        currCoords[1] += dirs[i][1];

        //        if (IsCoordsOnBoard(currCoords[0], currCoords[1]) && Board.Cells[currCoords[0], currCoords[1]].Occupant == null)
        //        {
        //            BoardCell finish = Board.Cells[currCoords[0], currCoords[1]];
        //            moves.Add(new Move(curr, finish, CurrPlayer, null));

        //            if (!CellsAble.Contains(curr))
        //            {
        //                CellsAble.Add(curr);
        //            }

        //            if (!IsCoordsOnBoard(currCoords[0] + dirs[i][0], currCoords[1] + dirs[i][1]))
        //            {
        //                i++;

        //                currCoords[0] = kX;
        //                currCoords[1] = kY;

        //                Moves.Add(new KingMove(curr, finish, CurrPlayer, null, moves));

        //                moves = new List<Move>();
        //            }
        //        }
        //    }
        //}

        public void AddMovesKing(Unit king)
        {
            int yDir = king.MoveDir;
            int kX = king.Xcoord;
            int kY = king.Ycoord;

            BoardCell curr = Board.Cells[kX, kY];

            int[] currCoords = new int[2];
            currCoords[0] = kX;
            currCoords[1] = kY;

            int[][] dirs = new int[4][];

            dirs[0] = new int[2] { 1, yDir };
            dirs[1] = new int[2] { -1, yDir };
            dirs[2] = new int[2] { 1, -yDir };
            dirs[3] = new int[2] { -1, -yDir };

            for (int i = 0; i < dirs.Length; i++)
            {
                int cX = currCoords[0] + dirs[i][0];
                int cY = currCoords[1] + dirs[i][1];

                if (IsCoordsOnBoard(cX, cY))
                {
                    if (Board.Cells[cX, cY].Occupant == null)
                    {
                        BoardCell finish = Board.Cells[cX, cY];

                        int opponentIdx = (Turn + 1) % 2;
                        Player opponent = GetPlayer(opponentIdx);
                        Moves.Add(new Move(curr, finish, CurrPlayer, opponent));

                        if (!CellsAble.Contains(curr))
                        {
                            CellsAble.Add(curr);
                        }
                    }
                }
            }
        }

        public bool IsCoordsOnBoard(int x, int y)
        {
            return (0 <= x && x < Board.Cells.GetLength(0)) && (0 <= y && y < Board.Cells.GetLength(1));
        }
    }
}
