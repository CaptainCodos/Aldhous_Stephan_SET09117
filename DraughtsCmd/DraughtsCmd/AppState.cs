using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    class AppState
    {
        public virtual void UpdateState()
        {
        }

        public virtual void ConstantDisplay()
        {

        }

        public virtual void Confirm(out int val)
        {
            Console.WriteLine("\nAre you sure? y/n");

            val = 0;

            switch (Input.GetLine())
            {
                case "y":
                    val = 1;
                    break;
                case "n":
                    val = 0;
                    break;
                default:
                    Confirm(out val);
                    break;
            }
        }
    }

    class MainMenu : AppState
    {
        private string m_winnerTxt;

        public MainMenu()
        {
            m_winnerTxt = "";

            if (Program.lastGame != null)
            {
                if (Program.lastGame.GameEnded)
                {
                    m_winnerTxt = "P" + (Program.lastGame.Winner + 1) + " winner!";
                }
            }
        }

        public override void UpdateState()
        {
            Console.Clear();
            Console.WriteLine("Crazy Draughts");

            Console.WriteLine("\n" + m_winnerTxt);

            Console.WriteLine("\nStart New Game: ng");
            Console.WriteLine("Load Game: lg");
            Console.WriteLine("Exit: e");
            Console.WriteLine("\nPlease choose action: ");

            int confirmVal = 0;
            
            switch(Input.GetLine())
            {
                case "ng":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        Game game = new Game();
                        Program.lastGame = game;

                        AppState state = this;
                        state = new StartPlay(game, 1);
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                case "lg":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        Console.WriteLine("\nPlease enter the game you want to load! [enter game number]");

                        string input = Input.GetLine();

                        int val;
                        int.TryParse(input, out val);
                        val -= 1;

                        if (val < Program.games.Count && Program.games.Count > 0)
                        {
                            AppState state = this;
                            state = new StartPlay(val);
                            state.UpdateState();
                        }
                        else
                        {
                            Console.WriteLine("\nThis game does not exist! Press enter to continue...");
                            Input.GetLine();
                            UpdateState();
                        }
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                case "e":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    UpdateState();
                    break;
            }
        }
    }

    class StartPlay : AppState
    {
        private Game m_game;
        private BoardCell m_playCell;
        private List<Move> m_cellMoves;
        private Move m_selectedMove;

        public StartPlay(int game)
        {
            m_game = Program.games[game];
            Program.lastGame = m_game;
        }

        public StartPlay(Game game, int newGameVal)
        {
            if (newGameVal == 0)
                m_game = game;
            else
            {
                m_game = game;
                Program.games.Add(game);
            }

            AppState state = this;
            if (m_game.GameEnded)
            {
                state = new MainMenu();
                state.UpdateState();
            }
        }

        public override void UpdateState()
        {
            ConstantDisplay();

            Console.WriteLine("\nPlay turn: p");
            Console.WriteLine("\nUndo: u");
            Console.WriteLine("Redo: r");
            Console.WriteLine("\nExit Game: e");

            int confirmVal;

            AppState state = this;

            switch (Input.GetLine())
            {
                case "p":
                    state = new SelectPiece(m_game);
                    state.UpdateState();
                    break;
                case "e":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        state = new MainMenu();
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    UpdateState();
                    break;
            }
        }

        public override void ConstantDisplay()
        {
            Console.Clear();
            Console.WriteLine("Game On!");
            Console.WriteLine("\nTurn " + (m_game.GetManager().Turn + 1));
            Console.WriteLine("Player: " + ((m_game.GetManager().Turn % 2) + 1));
            Console.WriteLine("");

            m_game.GetManager().Board.DrawBoard();

            Console.WriteLine("\nPlayer1 Units Remaining: " + m_game.GetManager().GetPlayer(0).Units);
            Console.WriteLine("Player1 Kills: " + m_game.GetManager().GetPlayer(0).Kills);

            Console.WriteLine("\nPlayer2 Units Remaining: " + m_game.GetManager().GetPlayer(1).Units);
            Console.WriteLine("Player2 Kills: " + m_game.GetManager().GetPlayer(1).Kills);
        }
    }

    class SelectPiece : AppState
    {
        private Game m_game;

        public SelectPiece(Game game)
        {
            m_game = game;
        }

        public override void UpdateState()
        {
            ConstantDisplay();

            int confirmVal;

            AppState state = this;

            string input = Input.GetLine();

            switch (input)
            {
                case "c":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        m_game.GetManager().CancelTurn();
                        state = new StartPlay(m_game, 0);
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    if (input.Length >= 3)
                    {
                        int x;
                        string testX = "" + input[0];
                        if (!int.TryParse(testX, out x))
                        {
                            UpdateState();
                        }

                        int y = m_game.GetManager().Board.ConvertYAxisToInt(input[2]);

                        if (m_game.GetManager().IsCoordsOnBoard(x, y) && m_game.GetManager().CellsAble.Contains(m_game.GetManager().Board.Cells[x, y]))
                        {
                            List<Move> moves = m_game.GetManager().GetMovesOfCell(m_game.GetManager().Board.Cells[x, y]);

                            if (moves.Count >= 0)
                            {
                                state = new SelectMove(m_game, moves);
                                state.UpdateState();
                            }
                            else
                            {
                                m_game.GetManager().FinishTurn();

                                state = new StartPlay(m_game, 0);
                                state.UpdateState();
                            }
                        }
                        else
                        {
                            m_game.GetManager().CancelTurn();
                            UpdateState();
                        }
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
            }
        }

        public override void ConstantDisplay()
        {
            Console.Clear();
            Console.WriteLine("Game On!");
            Console.WriteLine("\nTurn " + (m_game.GetManager().Turn + 1));
            Console.WriteLine("Player: " + ((m_game.GetManager().Turn % 2) + 1));
            Console.WriteLine("");

            m_game.GetManager().Board.DrawBoard();

            Console.WriteLine("\nPlayer1 Units Remaining: " + m_game.GetManager().GetPlayer(0).Units);
            Console.WriteLine("Player1 Kills: " + m_game.GetManager().GetPlayer(0).Kills);

            Console.WriteLine("\nPlayer2 Units Remaining: " + m_game.GetManager().GetPlayer(1).Units);
            Console.WriteLine("Player2 Kills: " + m_game.GetManager().GetPlayer(1).Kills);

            m_game.GetManager().GetUnits();

            string line = "\nPieces Available: ";

            for (int i = 0; i < m_game.GetManager().CellsAble.Count; i++)
            {
                if (i != 0)
                {
                    line += ", [" + m_game.GetManager().CellsAble[i].XCoord + "," + m_game.GetManager().Board.ConvertYAxisToChar(m_game.GetManager().CellsAble[i].YCoord) + "]";
                }
                else
                {
                    line += "[" + m_game.GetManager().CellsAble[i].XCoord + "," + m_game.GetManager().Board.ConvertYAxisToChar(m_game.GetManager().CellsAble[i].YCoord) + "]";
                }
            }

            Console.WriteLine(line);
            Console.WriteLine("\nSelectPiece: (Enter in form of 'letter,number')");
            Console.WriteLine("\nCancel Move: c");
        }
    }

    class SelectMove : AppState
    {
        private Game m_game;
        private BoardCell m_playCell;
        private List<Move> m_moves;
        private string m_errorTxt;

        public SelectMove(Game game, List<Move> moves)
        {
            m_game = game;
            m_moves = moves;
            m_errorTxt = "";
        }

        public override void UpdateState()
        {
            ConstantDisplay();

            int confirmVal;

            AppState state = this;

            string input = Input.GetLine();

            switch (input)
            {
                case "c":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        m_game.GetManager().CancelTurn();
                        state = new StartPlay(m_game, 0);
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    int select;
                    string testS = input;
                    if (int.TryParse(input, out select))
                    {
                        if (0 <= select && select < m_moves.Count)
                        {
                            //m_moves[select].ExecuteMove(1);
                            m_game.GetManager().ConductTurn(m_moves[select]);

                            List<Move> moves;

                            if (m_game.GetManager().CellsAble.Count > 0)
                            {
                                moves = m_game.GetManager().GetMovesOfCell(m_game.GetManager().CellsAble[0]);

                                state = new SelectMove(m_game, moves);
                                state.UpdateState();
                            }
                            else
                            {
                                m_game.GetManager().FinishTurn();

                                m_game.CheckIfComplete();

                                state = new StartPlay(m_game, 0);
                                state.UpdateState();
                            }
                        }
                        else
                        {
                            m_errorTxt = "Invalid Selection!";
                            UpdateState();
                        }
                    }
                    else
                    {
                        m_errorTxt = "Is not a number!";
                        UpdateState();
                    }
                    break;
            }
        }

        public override void ConstantDisplay()
        {
            Console.Clear();
            Console.WriteLine("Game On!");
            Console.WriteLine("\nTurn " + (m_game.GetManager().Turn + 1));
            Console.WriteLine("Player: " + ((m_game.GetManager().Turn % 2) + 1));
            Console.WriteLine("");

            m_game.GetManager().Board.DrawBoard(m_moves);

            Console.WriteLine("\nPlayer1 Units Remaining: " + m_game.GetManager().GetPlayer(0).Units);
            Console.WriteLine("Player1 Kills: " + m_game.GetManager().GetPlayer(0).Kills);

            Console.WriteLine("\nPlayer2 Units Remaining: " + m_game.GetManager().GetPlayer(1).Units);
            Console.WriteLine("Player2 Kills: " + m_game.GetManager().GetPlayer(1).Kills);

            Console.WriteLine("\n" + m_errorTxt);

            Console.WriteLine("\nSelectMove!");
            Console.WriteLine("\nCancel Move: c");
        }
    }

    class SelectDist : AppState
    {
        private Game m_game;
        private BoardCell m_playCell;
        private Move m_move;
        private string m_errorTxt;

        public SelectDist(Game game, Move move)
        {
            m_game = game;
            m_move = move;
            m_errorTxt = "";
        }

        public override void UpdateState()
        {
            ConstantDisplay();

            int confirmVal;

            AppState state = this;

            string input = Input.GetLine();

            switch (input)
            {
                case "c":
                    Confirm(out confirmVal);

                    if (confirmVal == 1)
                    {
                        state = new StartPlay(m_game, 0);
                        state.UpdateState();
                    }
                    else
                    {
                        UpdateState();
                    }
                    break;
                default:
                    int select;
                    string testS = input;
                    if (int.TryParse(input, out select))
                    {
                        KingMove kMove = (KingMove)m_move;
                        if (1 <= select && select < kMove.Moves.Count)
                        {
                            //m_game.GetManager().ConductTurn()

                            m_game.GetManager().FinishTurn();

                            state = new StartPlay(m_game, 0);
                            state.UpdateState();
                        }
                        else
                        {
                            m_errorTxt = "Invalid Selection!";
                            UpdateState();
                        }
                    }
                    else
                    {
                        m_errorTxt = "Is not a number!";
                        UpdateState();
                    }
                    break;
            }
        }

        public override void ConstantDisplay()
        {
            Console.Clear();
            Console.WriteLine("Game On!");
            Console.WriteLine("\nTurn " + (m_game.GetManager().Turn + 1));
            Console.WriteLine("Player: " + ((m_game.GetManager().Turn % 2) + 1));
            Console.WriteLine("");

            m_game.GetManager().Board.DrawBoard(m_move);

            Console.WriteLine("\nPlayer1 Units Remaining: " + m_game.GetManager().GetPlayer(0).Units);
            Console.WriteLine("Player1 Kills: " + m_game.GetManager().GetPlayer(0).Kills);

            Console.WriteLine("\nPlayer2 Units Remaining: " + m_game.GetManager().GetPlayer(1).Units);
            Console.WriteLine("Player2 Kills: " + m_game.GetManager().GetPlayer(1).Kills);

            Console.WriteLine("\n" + m_errorTxt);

            Console.WriteLine("\nSelectMove: ");
            Console.WriteLine("\nCancel Move: c");
        }
    }
}
