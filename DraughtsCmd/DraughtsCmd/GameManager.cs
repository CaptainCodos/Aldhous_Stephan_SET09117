using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class GameManager
    {
        public Player[] Players { get; set; } // Players in the game (2)

        public GameBoard Board { get; set; } // Gameboard played on

        public int Turn { get; set; } // Current turn
        public Player CurrPlayer { get; set; } // Current player

        public List<Move> Attacks { get; set; } // Attacks possible
        public List<Move> Moves { get; set; } // Moves possible
        public List<BoardCell> CellsAble { get; set; } // Cells able to move

        private Game m_game; // game this manages

        private GameHistory m_history; // history to use

        public GameManager(Game game, bool hasAI)
        {
            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();

            m_game = game;

            m_history = new GameHistory();

            Players = new Player[2];

            Players[0] = new Player("O", false);
            Players[1] = new Player("X", false);

            if (hasAI)
                Players[1].IsAIPlayer = true;

            Board = new GameBoard(this);

            Turn = 0;

            CurrPlayer = Players[Turn % 2];
        }

        // get player given index
        public Player GetPlayer(int player)
        {
            return Players[player];
        }

        // Executes given move and then checks if its possible to make another move
        public void ConductTurn(Move move)
        {
            move.ExecuteMove(1);

            BoardCell curr = move.GetEndCell;
            CellsAble.Add(curr);

            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();

            // if move is attack, check for further moves (only if piece did not just turn into a king)
            if (move is Attack && !move.GetEndCell.Occupant.Converted)
            {
                AddPossibleAttacks(new Coord(curr.XCoord, curr.YCoord));

                if (CurrPlayer.IsAIPlayer)
                {
                    AITurn();
                }
            }

            move.GetEndCell.Occupant.Converted = false;

            // if no cells available or move was normal and is AI player then finish
            if (CurrPlayer.IsAIPlayer && CellsAble.Count <= 0)
            {
                FinishTurn();
            }
        }

        // gets rid of available moves and cells
        public void CancelTurn()
        {
            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();
        }

        // Completes turn
        public void FinishTurn()
        {
            Attacks = new List<Move>();
            Moves = new List<Move>();
            CellsAble = new List<BoardCell>();

            // increment turn
            Turn += 1;
            CurrPlayer = Players[Turn % 2];

            // if player next turn is AI then execute AI move (requires no input and thus no game state changes)
            if (CurrPlayer.IsAIPlayer)
            {
                // add board to history before it begins
                AddCurrentMoment();

                // get cells that can move
                GetUnits();

                // conduct AI turn
                AITurn();
            }
        }

        // performs turn for AI
        public void AITurn()
        {
            // if there are any units that can move then check random available unit for possible moves
            if (CellsAble.Count > 0)
            {
                int piece = Program.RNG.Next(0, CellsAble.Count);
                List<Move> moves = GetMovesOfCell(CellsAble[piece]);

                // (sanity check first) pick random move and conduct the turn
                if (moves.Count > 0)
                {
                    int moveSelected = Program.RNG.Next(0, moves.Count);
                    Move move = moves[moveSelected];

                    ConductTurn(move);
                }
                else
                {
                    FinishTurn();
                }
            }
            else
            {
                FinishTurn();
            }
        }
        
        // Find all the cells with units that can move (attacks and normal moves are mutually exclusive)
        public void GetUnits()
        {
            for (int i = 0; i < CurrPlayer.ArmyUnits.Count; i++)
            {
                AddPossibleAttacks(CurrPlayer.ArmyUnits[i].UnitCoords);
            }

            if (Attacks.Count < 1)
            {
                for (int i = 0; i < CurrPlayer.ArmyUnits.Count; i++)
                {
                    AddPossibleMoves(CurrPlayer.ArmyUnits[i].UnitCoords);
                }
            }
        }

        // gets all available moves for cell in which this cell is the starting cell
        // checks for attacks first. If there are none checks for normal moves (attacks and normal moves are mutually exclusive)
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

        // adds possible attacks for board coordinate
        public void AddPossibleAttacks(Coord coords)
        {
            if (Board.Cells[coords.X, coords.Y].Occupant is Man)
            {
                AddAttacksMan(Board.Cells[coords.X, coords.Y].Occupant);
            }
            else
            {
                AddAttacksKing(Board.Cells[coords.X, coords.Y].Occupant);
            }
        }

        // adds possible attacks for man unit
        public void AddAttacksMan(Unit man)
        {
            int yDir = man.MoveDir;
            Coord manXY = new Coord(man.UnitCoords.X, man.UnitCoords.Y);

            BoardCell curr = Board.Cells[manXY.X, manXY.Y];

            // check if cell in -> dir is on board and contains unit
            // check if cell in -> 2*dir is on board and doesn't contain unit
            Coord dir = new Coord(1, yDir);
            if (IsCoordsOnBoard(manXY + dir))
            {
                Coord next = manXY + dir;
                if (Board.Cells[next.X, next.Y].Occupant != null && Board.Cells[next.X, next.Y].Occupant.Commander != CurrPlayer)
                {
                    BoardCell defender = Board.Cells[next.X, next.Y];

                    Coord final = next + dir;
                    if (IsCoordsOnBoard(final) && Board.Cells[final.X, final.Y].Occupant == null)
                    {
                        BoardCell finish = Board.Cells[final.X, final.Y];
                        Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

                        // Ensure cells that can move has no duplicates
                        if (!CellsAble.Contains(curr))
                        {
                            CellsAble.Add(curr);
                        }
                    }
                }
            }

            dir = new Coord(-1, yDir);
            if (IsCoordsOnBoard(manXY + dir))
            {
                Coord next = manXY + dir;
                if (Board.Cells[next.X, next.Y].Occupant != null && Board.Cells[next.X, next.Y].Occupant.Commander != CurrPlayer)
                {
                    BoardCell defender = Board.Cells[next.X, next.Y];

                    Coord final = next + dir;
                    if (IsCoordsOnBoard(final) && Board.Cells[final.X, final.Y].Occupant == null)
                    {
                        BoardCell finish = Board.Cells[final.X, final.Y];
                        Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

                        // Ensure cells that can move has no duplicates
                        if (!CellsAble.Contains(curr))
                        {
                            CellsAble.Add(curr);
                        }
                    }
                }
            }
        }

        // adds possible attacks for king unit
        public void AddAttacksKing(Unit king)
        {
            int yDir = king.MoveDir;

            Coord kCoord = new Coord(king.UnitCoords.X, king.UnitCoords.Y);

            BoardCell curr = Board.Cells[kCoord.X, kCoord.Y];

            Coord currCoords = kCoord;

            Coord[] dirs = new Coord[4];

            dirs[0] = new Coord(1, yDir);
            dirs[1] = new Coord(-1, yDir);
            dirs[2] = new Coord(1, -yDir);
            dirs[3] = new Coord(-1, -yDir);

            // for every dir, check if -> dir has enemy and is on board and check if -> 2*dir is empty and is on board
            for (int i = 0; i < dirs.Length; i++)
            {
                Coord cXY = currCoords + dirs[i];

                if (IsCoordsOnBoard(cXY))
                {
                    if (Board.Cells[cXY.X, cXY.Y].Occupant != null && Board.Cells[cXY.X, cXY.Y].Occupant.Commander != CurrPlayer)
                    {
                        BoardCell defender = Board.Cells[cXY.X, cXY.Y];

                        if (IsCoordsOnBoard(cXY + dirs[i]))
                        {
                            Coord final = cXY + dirs[i];
                            if (Board.Cells[final.X, final.Y].Occupant == null)
                            {
                                BoardCell finish = Board.Cells[final.X, final.Y];

                                Attacks.Add(new Attack(curr, finish, defender, CurrPlayer, defender.Occupant.Commander));

                                // Ensure cells that can move has no duplicates
                                if (!CellsAble.Contains(curr))
                                {
                                    CellsAble.Add(curr);
                                }
                            }
                        }
                    }
                }
            }
        }

        // adds possible moves for board coordinate
        public void AddPossibleMoves(Coord coords)
        {
            if (Board.Cells[coords.X, coords.Y].Occupant is Man)
            {
                AddMovesMan(Board.Cells[coords.X, coords.Y].Occupant);
            }
            else
            {
                AddMovesKing(Board.Cells[coords.X, coords.Y].Occupant);
            }
        }

        // adds possible moves for current man unit
        public void AddMovesMan(Unit man)
        {
            int yDir = man.MoveDir;
            Coord manXY = new Coord(man.UnitCoords.X, man.UnitCoords.Y);

            BoardCell curr = Board.Cells[manXY.X, manXY.Y];

            // check if cell in -> dir is empty and on board
            if (IsCoordsOnBoard(manXY + new Coord(1, yDir)))
            {
                Coord next = manXY + new Coord(1, yDir);
                if (Board.Cells[next.X, next.Y].Occupant == null)
                {
                    BoardCell finish = Board.Cells[next.X, next.Y];
                    int opponentIdx = (Turn + 1) % 2;
                    Player opponent = GetPlayer(opponentIdx);
                    Moves.Add(new Move(curr, finish, CurrPlayer, opponent));

                    // Ensure cells that can move has no duplicates
                    if (!CellsAble.Contains(curr))
                    {
                        CellsAble.Add(curr);
                    }
                }
            }

            if (IsCoordsOnBoard(manXY + new Coord(-1, yDir)))
            {
                Coord next = manXY + new Coord(-1, yDir);
                if (Board.Cells[next.X, next.Y].Occupant == null)
                {
                    BoardCell finish = Board.Cells[next.X, next.Y];
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

        // adds possible moves for king unit
        public void AddMovesKing(Unit king)
        {
            int yDir = king.MoveDir;
            Coord kCoord = new Coord(king.UnitCoords.X, king.UnitCoords.Y);

            BoardCell curr = Board.Cells[kCoord.X, kCoord.Y];

            Coord currCoords = kCoord;

            Coord[] dirs = new Coord[4];

            dirs[0] = new Coord(1, yDir);
            dirs[1] = new Coord(-1, yDir);
            dirs[2] = new Coord(1, -yDir);
            dirs[3] = new Coord(-1, -yDir);

            // for every dir check if cell in -> dir is empty and on board
            for (int i = 0; i < dirs.Length; i++)
            {
                Coord cXY = currCoords + dirs[i];

                if (IsCoordsOnBoard(cXY))
                {
                    if (Board.Cells[cXY.X, cXY.Y].Occupant == null)
                    {
                        BoardCell finish = Board.Cells[cXY.X, cXY.Y];

                        int opponentIdx = (Turn + 1) % 2;
                        Player opponent = GetPlayer(opponentIdx);
                        Moves.Add(new Move(curr, finish, CurrPlayer, opponent));

                        // Ensure cells that can move has no duplicates
                        if (!CellsAble.Contains(curr))
                        {
                            CellsAble.Add(curr);
                        }
                    }
                }
            }
        }

        // Checks if the coordinate is on the game board
        public bool IsCoordsOnBoard(Coord coords)
        {
            return (0 <= coords.X && coords.X < Board.Cells.GetLength(0)) && (0 <= coords.Y && coords.Y < Board.Cells.GetLength(1));
        }

        // adds units currently on the board to history
        public void AddCurrentMoment()
        {
            HistoryItem item = new HistoryItem();

            Player p1 = GetPlayer(0);
            Player p2 = GetPlayer(1);
            for (int i = 0; i < p1.ArmyUnits.Count; i++)
            {
                if (p1.ArmyUnits[i] is Man)
                {
                    item.AddUnit(0, p1.ArmyUnits[i].UnitCoords, 0, p1.Kills, Turn);
                }
                else
                {
                    item.AddUnit(0, p1.ArmyUnits[i].UnitCoords, 1, p1.Kills, Turn);
                }
            }

            for (int i = 0; i < p2.ArmyUnits.Count; i++)
            {
                if (p2.ArmyUnits[i] is Man)
                {
                    item.AddUnit(1, p2.ArmyUnits[i].UnitCoords, 0, p1.Kills, Turn);
                }
                else
                {
                    item.AddUnit(1, p2.ArmyUnits[i].UnitCoords, 1, p1.Kills, Turn);
                }
            }

            m_history.AddItem(item);
        }

        // clears the board and armies and replaces the pieces with ones stored in undo history item
        public void Undo()
        {
            Player p1 = Players[0];
            Player p2 = Players[1];
            for (int i = 0; i < p1.ArmyUnits.Count; i++)
            {
                Unit unit = p1.ArmyUnits[i];
                BoardCell cell = Board.Cells[unit.UnitCoords.X, unit.UnitCoords.Y];

                cell.EmptyCell();
            }

            for (int i = 0; i < p2.ArmyUnits.Count; i++)
            {
                Unit unit = p2.ArmyUnits[i];
                BoardCell cell = Board.Cells[unit.UnitCoords.X, unit.UnitCoords.Y];

                cell.EmptyCell();
            }

            p1.ArmyUnits = new List<Unit>();
            p1.Kills = 0;

            p2.ArmyUnits = new List<Unit>();
            p2.Kills = 0;

            m_history.Undo();

            HistoryItem item = m_history.CurrItem;

            p1.Kills = item.P1Kills;
            p2.Kills = item.P2Kills;
            for (int i = 0; i < item.P1UnitCoords.Count; i++)
            {
                BoardCell cell = Board.Cells[item.P1UnitCoords[i].X, item.P1UnitCoords[i].Y];
                switch (item.P1UnitTypes[i])
                {
                    case 0:
                        Man man = new Man(item.P1UnitCoords[i].X, item.P1UnitCoords[i].Y, p1);
                        man.MoveDir = -1;

                        cell.FillCell(man);
                        break;
                    case 1:
                        King king = new King(item.P1UnitCoords[i].X, item.P1UnitCoords[i].Y, p1);
                        king.MoveDir = -1;

                        cell.FillCell(king);
                        break;
                }
            }

            for (int i = 0; i < item.P2UnitCoords.Count; i++)
            {
                BoardCell cell = Board.Cells[item.P2UnitCoords[i].X, item.P2UnitCoords[i].Y];
                switch (item.P2UnitTypes[i])
                {
                    case 0:
                        Man man = new Man(item.P2UnitCoords[i].X, item.P2UnitCoords[i].Y, p2);
                        man.MoveDir = 1;

                        cell.FillCell(man);
                        break;
                    case 1:
                        King king = new King(item.P2UnitCoords[i].X, item.P2UnitCoords[i].Y, p2);
                        king.MoveDir = 1;

                        cell.FillCell(king);
                        break;
                }
            }

            Turn = item.Turn;
            CurrPlayer = Players[Turn % 2];
        }

        // clears the board and armies and replaces the pieces with ones stored in redo history item
        public void Redo()
        {
            Player p1 = Players[0];
            Player p2 = Players[1];
            for (int i = 0; i < p1.ArmyUnits.Count; i++)
            {
                Unit unit = p1.ArmyUnits[i];
                BoardCell cell = Board.Cells[unit.UnitCoords.X, unit.UnitCoords.Y];

                cell.EmptyCell();
            }

            for (int i = 0; i < p2.ArmyUnits.Count; i++)
            {
                Unit unit = p2.ArmyUnits[i];
                BoardCell cell = Board.Cells[unit.UnitCoords.X, unit.UnitCoords.Y];

                cell.EmptyCell();
            }

            p1.ArmyUnits = new List<Unit>();
            p1.Kills = 0;

            p2.ArmyUnits = new List<Unit>();
            p2.Kills = 0;

            m_history.Redo();

            HistoryItem item = m_history.CurrItem;

            p1.Kills = item.P1Kills;
            p2.Kills = item.P2Kills;
            for (int i = 0; i < item.P1UnitCoords.Count; i++)
            {
                BoardCell cell = Board.Cells[item.P1UnitCoords[i].X, item.P1UnitCoords[i].Y];
                switch (item.P1UnitTypes[i])
                {
                    case 0:
                        Man man = new Man(item.P1UnitCoords[i].X, item.P1UnitCoords[i].Y, p1);
                        man.MoveDir = -1;

                        cell.FillCell(man);
                        break;
                    case 1:
                        King king = new King(item.P1UnitCoords[i].X, item.P1UnitCoords[i].Y, p1);
                        king.MoveDir = -1;

                        cell.FillCell(king);
                        break;
                }
            }

            for (int i = 0; i < item.P2UnitCoords.Count; i++)
            {
                BoardCell cell = Board.Cells[item.P2UnitCoords[i].X, item.P2UnitCoords[i].Y];
                switch (item.P2UnitTypes[i])
                {
                    case 0:
                        Man man = new Man(item.P2UnitCoords[i].X, item.P2UnitCoords[i].Y, p2);
                        man.MoveDir = 1;

                        cell.FillCell(man);
                        break;
                    case 1:
                        King king = new King(item.P2UnitCoords[i].X, item.P2UnitCoords[i].Y, p2);
                        king.MoveDir = 1;

                        cell.FillCell(king);
                        break;
                }
            }

            Turn = item.Turn;
            CurrPlayer = Players[Turn % 2];
        }
    }
}
