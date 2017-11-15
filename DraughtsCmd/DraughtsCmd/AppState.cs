using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsCmd
{
    /// <summary>
    /// Base state to be used the the app's state machine
    /// 
    ///     MainMenu -----> StartPlay -----> SelectPiece -----> SelectMove
    ///                         ^                 |                  |
    ///                         |              cancel              cancel
    ///                         |                 v                  v
    ///                         --------------------------------------
    /// 
    /// Any input entered in any state that is not specified in it's respective switch
    /// statement, will result in regressing back to the start of that state.
    /// </summary>
    class AppState
    {
        public virtual void UpdateState()
        {
        }

        public virtual void ConstantDisplay()
        {

        }

        // Used to comfirm a selection
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
                    // re-ask the question if input is not y or n
                    Confirm(out val);
                    break;
            }
        }
    }

    /// <summary>
    /// App main menu
    /// </summary>
    class MainMenu : AppState
    {
        // text to display the winner of the last played game
        private string m_winnerTxt;

        public MainMenu()
        {
            m_winnerTxt = "";

            // set winner announcement if the last game has ended
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
            
            // choose option specified above (e.g. Start New Game = ng)
            switch(Input.GetLine())
            {
                case "ng":
                    Confirm(out confirmVal);
                    
                    if (confirmVal == 1)
                    {
                        Console.WriteLine("\nFight against AI? y/n");

                        Game game;
                        AppState state = this;

                        // decide if game willbe versus an AI
                        switch (Input.GetLine())
                        {
                            case "y":
                                game = new Game(true);
                                Program.lastGame = game;

                                state = new StartPlay(game, 1);
                                state.UpdateState();
                                break;
                            case "n":
                                game = new Game(false);
                                Program.lastGame = game;
                                
                                state = new StartPlay(game, 1);
                                state.UpdateState();
                                break;
                            default:
                                UpdateState();
                                break;
                        }
                    }
                    else
                    {
                        // regress back to start of state
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

                        if (val < Program.games.Count && val >= 0 && Program.games.Count > 0)
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
                        // regress back to start of state
                        UpdateState();
                    }
                    break;
                case "e":
                    Confirm(out confirmVal);

                    // confirm if you want to exit the game
                    if (confirmVal == 1)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        // regress back to start of state
                        UpdateState();
                    }
                    break;
                default:
                    // invalid input results in regression back to start of state
                    UpdateState();
                    break;
            }
        }
    }

    /// <summary>
    /// Beginning of a turn, player can choose to play the turn, undo, redo or exit
    /// </summary>
    class StartPlay : AppState
    {
        private Game m_game;

        // load game from session
        public StartPlay(int game)
        {
            m_game = Program.games[game];
            Program.lastGame = m_game;
        }

        // start new game or resume current game
        public StartPlay(Game game, int newGameVal)
        {
            if (newGameVal == 0)
                m_game = game;
            else
            {
                m_game = game;
                Program.games.Add(game);

                m_game.GetManager().AddCurrentMoment();
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
                case "u":
                    m_game.GetManager().Undo();
                    UpdateState();
                    break;
                case "r":
                    m_game.GetManager().Redo();
                    UpdateState();
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

    /// <summary>
    /// If player chooses to play the turn then they must select a possible piece to move
    /// </summary>
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
                        // cancels the turn being taken (allows you to select different piece if possible)
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
                        // parses the x component of the cell you entered
                        int x;
                        string testX = "" + input[0];
                        if (!int.TryParse(testX, out x))
                        {
                            // regress if value cannot parse
                            UpdateState();
                        }

                        int y = m_game.GetManager().Board.ConvertYAxisToInt(input[2]);

                        if (m_game.GetManager().IsCoordsOnBoard(new Coord(x, y)) && m_game.GetManager().CellsAble.Contains(m_game.GetManager().Board.Cells[x, y]))
                        {
                            List<Move> moves = m_game.GetManager().GetMovesOfCell(m_game.GetManager().Board.Cells[x, y]);

                            // ensure that you can only go to SelectMove state if a move is available otherwise finish turn
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
                            // if input is invalid then regress
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

    /// <summary>
    /// The player must then select a move available to the piece they selected
    /// </summary>
    class SelectMove : AppState
    {
        private Game m_game;
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

                    // choose to cancel the current turn
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

                    // check if input can be parsed and if the input is within the range of moves
                    if (int.TryParse(input, out select))
                    {
                        select--;
                        if (0 <= select && select < m_moves.Count)
                        {
                            // conducts the move selected (this method also checks for possible attack if the move was an attack
                            m_game.GetManager().ConductTurn(m_moves[select]);

                            List<Move> moves;

                            // if there is an attack available then the unit that just attacked will be available
                            // meaning they now have the option to make another move(attack)
                            if (m_game.GetManager().CellsAble.Count > 0)
                            {
                                moves = m_game.GetManager().GetMovesOfCell(m_game.GetManager().CellsAble[0]);

                                m_game.GetManager().AddCurrentMoment();

                                state = new SelectMove(m_game, moves);
                                state.UpdateState();
                            }
                            else
                            {
                                m_game.GetManager().FinishTurn();

                                m_game.GetManager().AddCurrentMoment();

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
}
